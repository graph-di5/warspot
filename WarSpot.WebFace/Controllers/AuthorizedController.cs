using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WarSpot.WebFace.Security;

namespace WarSpot.WebFace.Controllers
{
	[Authorize]
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
				if (newTicket != null && newTicket.Expiration != ticket.Expiration)
				{
					string encryptedTicket = FormsAuthentication.Encrypt(newTicket);

					cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
					cookie.Path = FormsAuthentication.FormsCookiePath;
					Response.Cookies.Add(cookie);
				}
				CustomPrincipal.Login(ticket.UserData);
			}
		}
	}
}