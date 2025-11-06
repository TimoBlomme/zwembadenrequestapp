using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Mvc5;
using ZwembadenRequestApp.Core.Interfaces;
using ZwembadenRequestApp.Web.Models;
using ZwembadenRequestApp.Web.Services;

namespace ZwembadenRequestApp.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // Register your types here
            container.RegisterType<ApplicationDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IQuoteRequestService, QuoteRequestService>(new HierarchicalLifetimeManager());

            // Register Identity types
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(
                new HierarchicalLifetimeManager(),
                new InjectionConstructor(typeof(ApplicationDbContext)));

            container.RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<ApplicationUserManager>(new HierarchicalLifetimeManager());
            container.RegisterType<ApplicationSignInManager>(new HierarchicalLifetimeManager());

            // Set the dependency resolver for MVC
            System.Web.Mvc.DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}