using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using WarSpot.WebFace.Controllers;

namespace WarSpot.WebFace
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
			filters.Add(new AuthorizeAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
					"Default", // Route name
					"{controller}/{action}/{id}", // URL with parameters
					new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);

		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

			CloudStorageAccount.SetConfigurationSettingPublisher(
											(a, b) => b(RoleEnvironment.GetConfigurationSettingValue(a)));
		}

		protected void Application_Error(object sender, EventArgs e)
		{
			Exception exception = Server.GetLastError();
			Server.ClearError();

			RouteData routeData = new RouteData();
			routeData.Values.Add("controller", "Error");
			routeData.Values.Add("action", "Index");
			routeData.Values.Add("error", exception);

			if (exception.GetType() == typeof(HttpException))
			{
				routeData.Values.Add("statusCode", ((HttpException)exception).GetHttpCode());
			}
			else
			{
				routeData.Values.Add("statusCode", 500);
			}

			IController controller = new ErrorController();
			controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
		}
	}
}