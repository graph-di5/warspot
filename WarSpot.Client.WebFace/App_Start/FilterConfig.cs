using System.Web;
using System.Web.Mvc;

namespace WarSpot.Client.WebFace
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}