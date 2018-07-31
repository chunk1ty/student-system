using System.Web.Mvc;

using StudentSystem.Clients.Mvc.Infrastructure.Filters;

namespace StudentSystem.Clients.Mvc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new StudentSystemExeptionAttribute());
        }
    }
}
