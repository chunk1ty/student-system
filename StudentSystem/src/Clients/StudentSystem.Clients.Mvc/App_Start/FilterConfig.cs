using System.Web;
using System.Web.Mvc;

namespace StudentSystem.Clients.Mvc
{
    public class FilterConfig
    {
        //TODO add loging when exeption occurred 
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
