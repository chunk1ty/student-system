﻿using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace StudentSystem.Clients.Mvc
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngineConfig.RegisterViewEngine();
            DbConfig.RegisterDb();
           
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
