using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WarSpot.Client.WebFace.Filters;
using WarSpot.Client.WebFace.Models;
using WarSpot.Cloud.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WarSpot.Cloud.Storage;
using WebMatrix.WebData;
using WarSpot.Client.WebFace.Filters;
using WarSpot.Client.WebFace.Models;

namespace WarSpot.Client.WebFace.Controllers
{
	public interface ICustomPrincipal : IPrincipal
	{

	}

	public interface ICustomIdentity : IIdentity
	{
		bool IsInRole(string role);
		string ToJson();
	}

	/// <summary>
	/// Private members have short names to preserve space using json serialization
	/// </summary>
	public class IdentityRepresentation
	{
		private bool ia;

		public bool IsAuthenticated
		{
			get { return ia; }
			set { ia = value; }
		}

		private string n;

		public string Name
		{
			get { return n; }
			set { n = value; }
		}

		private string r;

		public string Roles
		{
			get { return r; }
			set { r = value; }
		}

	}

	public class CustomIdentity : ICustomIdentity
	{
		/// <summary>
		/// Authenticate and get identity out with roles
		/// </summary>
		/// <param name="userName">User name</param>
		/// <param name="password">Password</param>
		/// <returns>Instance of identity</returns>
		public static CustomIdentity GetCustomIdentity(string userName, string password)
		{
			CustomIdentity identity = new CustomIdentity();
			if (Membership.ValidateUser(userName, password))
			{
				identity.IsAuthenticated = true;
				identity.Name = userName;
				var roles = System.Web.Security.Roles.GetRolesForUser(userName);
				identity.Roles = roles;
				return identity;
			}
			return identity;
		}

		private CustomIdentity() { }

		public string AuthenticationType
		{
			get { return "Custom"; }
		}

		public bool IsAuthenticated { get; private set; }

		public string Name { get; private set; }

		private string[] Roles { get; set; }

		public bool IsInRole(string role)
		{
			if (string.IsNullOrEmpty(role))
			{
				throw new ArgumentException("Role is null");
			}
			return Roles.Where(one => one.ToUpper().Trim() == role.ToUpper().Trim()).Any();
		}

		/// <summary>
		/// Create serialized string for storing in a cookie
		/// </summary>
		/// <returns>String representation of identity</returns>
		public string ToJson()
		{
			string returnValue = string.Empty;
			IdentityRepresentation representation = new IdentityRepresentation()
			{
				IsAuthenticated = this.IsAuthenticated,
				Name = this.Name,
				Roles = string.Join("|", this.Roles)
			};
			DataContractJsonSerializer jsonSerializer =
					new DataContractJsonSerializer(typeof(IdentityRepresentation));
			using (MemoryStream stream = new MemoryStream())
			{
				jsonSerializer.WriteObject(stream, representation);
				stream.Flush();
				byte[] json = stream.ToArray();
				returnValue = Encoding.UTF8.GetString(json, 0, json.Length);
			}

			return returnValue;
		}

		/// <summary>
		/// Create identity from a cookie data
		/// </summary>
		/// <param name="cookieString">String stored in cookie, created via ToJson method</param>
		/// <returns>Instance of identity</returns>
		public static ICustomIdentity FromJson(string cookieString)
		{

			IdentityRepresentation serializedIdentity = null;
			using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(cookieString)))
			{
				DataContractJsonSerializer jsonSerializer =
						new DataContractJsonSerializer(typeof(IdentityRepresentation));
				serializedIdentity = jsonSerializer.ReadObject(stream) as IdentityRepresentation;
			}
			CustomIdentity identity = new CustomIdentity()
			{
				IsAuthenticated = serializedIdentity.IsAuthenticated,
				Name = serializedIdentity.Name,
				Roles = serializedIdentity.Roles
						.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)
			};
			return identity;
		}

	}

	public class CustomPrincipal : ICustomPrincipal
	{
		private CustomPrincipal() { }

		private CustomPrincipal(ICustomIdentity identity)
		{
			this.Identity = identity;
		}

		public IIdentity Identity { get; private set; }

		public bool IsInRole(string role)
		{
			if (string.IsNullOrEmpty(role))
			{
				throw new ArgumentException("Role is null");
			}
			return ((ICustomIdentity)Identity).IsInRole(role);
		}


		public static void Logout()
		{
			HttpContext.Current.User =
					new GenericPrincipal(new GenericIdentity(""), new string[] { });
		}

		/// <summary>
		/// Login
		/// </summary>
		/// <param name="userName">User name</param>
		/// <param name="password">Password</param>
		/// <param name="rememberMe">True, if authentication should persist between browser sessions
		/// </param>
		/// <returns>True if login succeeds</returns>
		public static bool Login(string userName, string password, bool rememberMe)
		{
			var identity = CustomIdentity.GetCustomIdentity(userName, password);
			if (identity.IsAuthenticated)
			{
				HttpContext.Current.User = new CustomPrincipal(identity);
				FormsAuthenticationTicket ticket =
							 new FormsAuthenticationTicket(
									 1, identity.Name, DateTime.Now, DateTime.Now.AddMinutes(30), rememberMe,
									 identity.ToJson(), FormsAuthentication.FormsCookiePath);
				string encryptedTicket = FormsAuthentication.Encrypt(ticket);

				var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
				cookie.Path = FormsAuthentication.FormsCookiePath;
				if (rememberMe)
				{
					cookie.Expires = DateTime.Now.AddYears(1);// good for one year
				}

				HttpContext.Current.Response.Cookies.Add(cookie);
			}
			return identity.IsAuthenticated;
		}

		public static bool Login(string cookieString)
		{
			ICustomIdentity identity = CustomIdentity.FromJson(cookieString);
			if (identity.IsAuthenticated)
			{
				HttpContext.Current.User = new CustomPrincipal(identity);
			}
			return identity.IsAuthenticated;
		}
	}

	[Authorize]
	[InitializeSimpleMembership]
	public class AuthorizedController : Controller
	{
		protected override void OnAuthorization(AuthorizationContext filterContext)
		{
			base.OnAuthorization(filterContext);
			HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
			if (cookie != null)
			{
				FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
				var newTicket = FormsAuthentication.RenewTicketIfOld(ticket);
				if (newTicket.Expiration != ticket.Expiration)
				{
					string encryptedTicket = FormsAuthentication.Encrypt(newTicket);

					cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
					cookie.Path = FormsAuthentication.FormsCookiePath;
					Response.Cookies.Add(cookie);
				}
				CustomPrincipal.Login(ticket.UserData);
			}
		}
		//
		// GET: /Account/Login

		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		//
		// POST: /Account/Login
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Login(LoginModel model, string returnUrl)
		{

			if (ModelState.IsValid)
			{
				logedIn = Warehouse.Login(model.UserName, model.Password);
				if (logedIn) //WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
				{
					return RedirectToLocal(returnUrl);
				}
			}

			// If we got this far, something failed, redisplay form
			ModelState.AddModelError("", "The user name or password provided is incorrect.");
			return View(model);
		}

		static private bool logedIn;

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			logedIn = false;
#if false
				_storage.logout
				WebSecurity.Logout();
#endif

			return RedirectToAction("Index", "Home");
		}

		//
		// GET: /Account/Register

		[AllowAnonymous]
		public ActionResult Register()
		{
			return View();
		}

		//
		// POST: /Account/Register

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Register(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				// Attempt to register the user
				try
				{
#if true
					Warehouse.Register(model.UserName, model.Password);
					logedIn = Warehouse.Login(model.UserName, model.Password);
					//if(!logedIn)
					//{
					//  return 
					//}
#else
						WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
						WebSecurity.Login(model.UserName, model.Password);
#endif
					return RedirectToAction("Index", "Home");
				}
				catch (MembershipCreateUserException e)
				{
					ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

	}

		//[Authorize]
		[InitializeSimpleMembership]
		public class AccountController : Controller
		{
		
			static private bool logedIn;
			//
			// GET: /Account/Login

			[AllowAnonymous]
			public ActionResult Login(string returnUrl)
			{
				ViewBag.ReturnUrl = returnUrl;
				return View();
			}

			//
			// POST: /Account/Login

			[HttpPost]
			[AllowAnonymous]
			[ValidateAntiForgeryToken]
			public ActionResult Login(LoginModel model, string returnUrl)
			{

				if (ModelState.IsValid)
				{
					logedIn = Warehouse.Login(model.UserName, model.Password);
					if (logedIn) //WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
					{
						return RedirectToLocal(returnUrl);
					}
				}

				// If we got this far, something failed, redisplay form
				ModelState.AddModelError("", "The user name or password provided is incorrect.");
				return View(model);
			}

			//
			// POST: /Account/LogOff

			[HttpPost]
			[ValidateAntiForgeryToken]
			public ActionResult LogOff()
			{
				logedIn = false;
	#if false
				_storage.logout
				WebSecurity.Logout();
	#endif

				return RedirectToAction("Index", "Home");
			}

			//
			// GET: /Account/Register

			[AllowAnonymous]
			public ActionResult Register()
			{
				return View();
			}

			//
			// POST: /Account/Register

			[HttpPost]
			[AllowAnonymous]
			[ValidateAntiForgeryToken]
			public ActionResult Register(RegisterModel model)
			{
				if (ModelState.IsValid)
				{
					// Attempt to register the user
					try
					{
	#if true
						Warehouse.Register(model.UserName, model.Password);
						logedIn = Warehouse.Login(model.UserName, model.Password);
						//if(!logedIn)
						//{
						//  return 
						//}
	#else
						WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
						WebSecurity.Login(model.UserName, model.Password);
	#endif
						return RedirectToAction("Index", "Home");
					}
					catch (MembershipCreateUserException e)
					{
						ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
					}
				}

				// If we got this far, something failed, redisplay form
				return View(model);
			}

			//
			// POST: /Account/Disassociate

			[HttpPost]
			[ValidateAntiForgeryToken]
			public ActionResult Disassociate(string provider, string providerUserId)
			{
				string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
				ManageMessageId? message = null;

				// Only disassociate the account if the currently logged in user is the owner
				if (ownerAccount == User.Identity.Name)
				{
					// Use a transaction to prevent the user from deleting their last login credential
					using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
					{
						bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
						if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
						{
							OAuthWebSecurity.DeleteAccount(provider, providerUserId);
							scope.Complete();
							message = ManageMessageId.RemoveLoginSuccess;
						}
					}
				}

				return RedirectToAction("Manage", new { Message = message });
			}

			//
			// GET: /Account/Manage

			public ActionResult Manage(ManageMessageId? message)
			{
				ViewBag.StatusMessage =
						message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
						: message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
						: message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
						: "";
				ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
				ViewBag.ReturnUrl = Url.Action("Manage");
				return View();
			}

			//
			// POST: /Account/Manage

			[HttpPost]
			[ValidateAntiForgeryToken]
			public ActionResult Manage(LocalPasswordModel model)
			{
				bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
				ViewBag.HasLocalPassword = hasLocalAccount;
				ViewBag.ReturnUrl = Url.Action("Manage");
				if (hasLocalAccount)
				{
					if (ModelState.IsValid)
					{
						// ChangePassword will throw an exception rather than return false in certain failure scenarios.
						bool changePasswordSucceeded;
						try
						{
							changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
						}
						catch (Exception)
						{
							changePasswordSucceeded = false;
						}

						if (changePasswordSucceeded)
						{
							return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
						}
						else
						{
							ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
						}
					}
				}
				else
				{
					// User does not have a local password so remove any validation errors caused by a missing
					// OldPassword field
					ModelState state = ModelState["OldPassword"];
					if (state != null)
					{
						state.Errors.Clear();
					}

					if (ModelState.IsValid)
					{
						try
						{
							WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
							return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
						}
						catch (Exception e)
						{
							ModelState.AddModelError("", e);
						}
					}
				}

				// If we got this far, something failed, redisplay form
				return View(model);
			}

			//
			// POST: /Account/ExternalLogin

			[HttpPost]
			[AllowAnonymous]
			[ValidateAntiForgeryToken]
			public ActionResult ExternalLogin(string provider, string returnUrl)
			{
				return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
			}

			//
			// GET: /Account/ExternalLoginCallback

			[AllowAnonymous]
			public ActionResult ExternalLoginCallback(string returnUrl)
			{
				AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
				if (!result.IsSuccessful)
				{
					return RedirectToAction("ExternalLoginFailure");
				}

				if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
				{
					return RedirectToLocal(returnUrl);
				}

				if (User.Identity.IsAuthenticated)
				{
					// If the current user is logged in add the new account
					OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
					return RedirectToLocal(returnUrl);
				}
				else
				{
					// User is new, ask for their desired membership name
					string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
					ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
					ViewBag.ReturnUrl = returnUrl;
					return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
				}
			}

			//
			// POST: /Account/ExternalLoginConfirmation

			[HttpPost]
			[AllowAnonymous]
			[ValidateAntiForgeryToken]
			public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
			{
				string provider = null;
				string providerUserId = null;

				if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
				{
					return RedirectToAction("Manage");
				}

				if (ModelState.IsValid)
				{
					// Insert a new user into the database
					using (UsersContext db = new UsersContext())
					{
						UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
						// Check if user already exists
						if (user == null)
						{
							// Insert name into the profile table
							db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
							db.SaveChanges();

							OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
							OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

							return RedirectToLocal(returnUrl);
						}
						else
						{
							ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
						}
					}
				}

				ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
				ViewBag.ReturnUrl = returnUrl;
				return View(model);
			}

			//
			// GET: /Account/ExternalLoginFailure

			[AllowAnonymous]
			public ActionResult ExternalLoginFailure()
			{
				return View();
			}

			[AllowAnonymous]
			[ChildActionOnly]
			public ActionResult ExternalLoginsList(string returnUrl)
			{
				ViewBag.ReturnUrl = returnUrl;
				return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
			}

			[ChildActionOnly]
			public ActionResult RemoveExternalLogins()
			{
				ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
				List<ExternalLogin> externalLogins = new List<ExternalLogin>();
				foreach (OAuthAccount account in accounts)
				{
					AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

					externalLogins.Add(new ExternalLogin
					{
						Provider = account.Provider,
						ProviderDisplayName = clientData.DisplayName,
						ProviderUserId = account.ProviderUserId,
					});
				}

				ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
				return PartialView("_RemoveExternalLoginsPartial", externalLogins);
			}

			#region Helpers
			private ActionResult RedirectToLocal(string returnUrl)
			{
				if (Url.IsLocalUrl(returnUrl))
				{
					return Redirect(returnUrl);
				}
				else
				{
					return RedirectToAction("Index", "Home");
				}
			}

			public enum ManageMessageId
			{
				ChangePasswordSuccess,
				SetPasswordSuccess,
				RemoveLoginSuccess,
			}

			internal class ExternalLoginResult : ActionResult
			{
				public ExternalLoginResult(string provider, string returnUrl)
				{
					Provider = provider;
					ReturnUrl = returnUrl;
				}

				public string Provider { get; private set; }
				public string ReturnUrl { get; private set; }

				public override void ExecuteResult(ControllerContext context)
				{
					// todo logedIn 
					OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
				}
			}

			private static string ErrorCodeToString(MembershipCreateStatus createStatus)
			{
				// See http://go.microsoft.com/fwlink/?LinkID=177550 for
				// a full list of status codes.
				switch (createStatus)
				{
				case MembershipCreateStatus.DuplicateUserName:
					return "User name already exists. Please enter a different user name.";

				case MembershipCreateStatus.DuplicateEmail:
					return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

				case MembershipCreateStatus.InvalidPassword:
					return "The password provided is invalid. Please enter a valid password value.";

				case MembershipCreateStatus.InvalidEmail:
					return "The e-mail address provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidAnswer:
					return "The password retrieval answer provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidQuestion:
					return "The password retrieval question provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidUserName:
					return "The user name provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.ProviderError:
					return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				case MembershipCreateStatus.UserRejected:
					return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				default:
					return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
				}
			}
			#endregion
		}
	 /**/
}
