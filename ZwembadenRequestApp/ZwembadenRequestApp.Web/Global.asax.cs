using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ZwembadenRequestApp.Web.App_Data;
using ZwembadenRequestApp.Core.Interfaces;
using ZwembadenRequestApp.Web.Services;

namespace ZwembadenRequestApp.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            // Register Unity DI container
            UnityConfig.RegisterComponents();

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Initialize database
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());

            using (var context = new ApplicationDbContext())
            {
                context.Database.Initialize(false);
            }
        }
    }
}