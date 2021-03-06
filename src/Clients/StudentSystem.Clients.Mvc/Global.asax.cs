﻿using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using StudentSystem.Infrastructure.Mapping;

namespace StudentSystem.Clients.Mvc
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngineConfig.RegisterViewEngine();
            AutoMapperConfig.RegisterAutomapper();
            StudentSystem.Persistence.DbConfig.RegisterDb();
            StudentSystem.Authentication.DbConfig.RegisterDb();
            log4net.Config.XmlConfigurator.Configure();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
