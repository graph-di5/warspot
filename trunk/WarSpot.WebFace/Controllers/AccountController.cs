using System.Web.Mvc;
using System.Web.Security;
using WarSpot.Cloud.Storage;
using WarSpot.WebFace.Models;
using WarSpot.WebFace.Security;

namespace WarSpot.WebFace.Controllers
{
	public class AccountController : Controller
	{

		//
		// GET: /Account/LogOn
		[AllowAnonymous]
		public ActionResult LogOn()
		{
			return View();
		}

		//
		// POST: /Account/LogOn

		[AllowAnonymous]
		[HttpPost]
		public ActionResult LogOn(LogOnModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				if (CustomPrincipal.Login(model.UserName, model.Password, model.RememberMe))
				{
					if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
						 && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
					{
						return Redirect(returnUrl);
					}
					else
					{
						return RedirectToAction("Index", "Home");
					}
				}
				else
				{
					ModelState.AddModelError("", "The user name or password provided is incorrect.");
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		//
		// GET: /Account/LogOff

		[AllowAnonymous]
		public ActionResult LogOff()
		{
			CustomPrincipal.Logout();

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

		[AllowAnonymous]
		[HttpPost]
		public ActionResult Register(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				// Attempt to register the user
				//MembershipCreateStatus createStatus;
				//Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);


				if (Warehouse.Register(model.UserName, model.Password))
				{
					if (CustomPrincipal.Login(model.UserName, model.Password, false))
					{
						return RedirectToAction("Index", "Home");
					}
					else
					{
						ModelState.AddModelError("", "The user name or password provided is incorrect.");
					}
				}
				else
				{
					// todo make error message
					ModelState.AddModelError("", "Some error while registration"/*ErrorCodeToString(createStatus)/**/);
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		//
		// GET: /Account/ChangePassword

		public ActionResult ChangePassword()
		{
			return View();
		}

		//
		// POST: /Account/ChangePassword

		// todo rewrite this
#if false
		[HttpPost]
		public ActionResult ChangePassword(ChangePasswordModel model)
		{
			if (ModelState.IsValid)
			{

				// ChangePassword will throw an exception rather
				// than return false in certain failure scenarios.
				bool changePasswordSucceeded;
				try
				{
					MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
					changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
				}
				catch (Exception)
				{
					changePasswordSucceeded = false;
				}

				if (changePasswordSucceeded)
				{
					return RedirectToAction("ChangePasswordSuccess");
				}
				else
				{
					ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}
#endif

		//
		// GET: /Account/ChangePasswordSuccess

		public ActionResult ChangePasswordSuccess()
		{
			return View();
		}

		#region Status Codes
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
}
