using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace WarSpot.WebFace.Security
{
	public class CustomPrincipal : ICustomPrincipal
	{
		private CustomPrincipal(ICustomIdentity identity)
		{
			Identity = identity;
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
			HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
			if (cookie != null)
			{
				FormsAuthentication.SignOut();
				HttpContext.Current.Request.Cookies.Remove(FormsAuthentication.FormsCookieName);
			}
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
}