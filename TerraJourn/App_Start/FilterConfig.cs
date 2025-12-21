using System.Web;
using System.Web.Mvc;
using TerraJourn.Filters;

namespace TerraJourn
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthFilter());
        }
    }
}
