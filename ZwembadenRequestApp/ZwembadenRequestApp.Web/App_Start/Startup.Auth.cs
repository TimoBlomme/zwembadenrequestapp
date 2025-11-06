using System;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using ZwembadenRequestApp.Core.Entities;
using ZwembadenRequestApp.Web.Models;

[assembly: OwinStartup(typeof(ZwembadenRequestApp.Web.Startup))]

namespace ZwembadenRequestApp.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers();
        }

        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }

        private void CreateRolesAndUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Create Admin Role
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                // Create Admin User
                var user = new ApplicationUser();
                user.UserName = "admin@pri.be";
                user.Email = "admin@pri.be";
                user.FirstName = "Admin";
                user.LastName = "User";

                string userPassword = "Test1237";

                var chkUser = userManager.Create(user, userPassword);
                if (chkUser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }

            // Create Customer Role
            if (!roleManager.RoleExists("Customer"))
            {
                var role = new IdentityRole();
                role.Name = "Customer";
                roleManager.Create(role);
            }

            // Seed customer users
            var customers = new[]
            {
                new { UserName = "customerA@pri.be", Email = "customerA@pri.be", FirstName = "Customer", LastName = "A" },
                new { UserName = "customerB@pri.be", Email = "customerB@pri.be", FirstName = "Customer", LastName = "B" },
                new { UserName = "customerC@pri.be", Email = "customerC@pri.be", FirstName = "Customer", LastName = "C" }
            };

            foreach (var customer in customers)
            {
                if (userManager.FindByName(customer.UserName) == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = customer.UserName,
                        Email = customer.Email,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName
                    };

                    var result = userManager.Create(user, "Test1237");
                    if (result.Succeeded)
                    {
                        userManager.AddToRole(user.Id, "Customer");
                    }
                }
            }

            // Seed quote requests
            SeedQuoteRequests(context);
        }

        private void SeedQuoteRequests(ApplicationDbContext context)
        {
            if (context.QuoteRequests.Any()) return;

            var customerA = context.Users.First(u => u.UserName == "customerA@pri.be");
            var customerB = context.Users.First(u => u.UserName == "customerB@pri.be");
            var customerC = context.Users.First(u => u.UserName == "customerC@pri.be");

            var quoteRequests = new[]
            {
                // CustomerA: 3 completed + 1 open
                new QuoteRequest { CustomerId = customerA.Id, CustomerName = customerA.UserName, PoolType = "Rechthoekig", Length = 10, Width = 5, Depth = 1.5m, NumberOfLights = 2, HasStairs = true, Status = "Done", ProposedPrice = 15000, RequestDate = DateTime.Now.AddDays(-10), ResponseDate = DateTime.Now.AddDays(-5) },
                new QuoteRequest { CustomerId = customerA.Id, CustomerName = customerA.UserName, PoolType = "Ovaal", Length = 8, Width = 4, Depth = 1.2m, NumberOfLights = 4, HasStairs = true, Status = "Done", ProposedPrice = 12000, RequestDate = DateTime.Now.AddDays(-20), ResponseDate = DateTime.Now.AddDays(-15) },
                new QuoteRequest { CustomerId = customerA.Id, CustomerName = customerA.UserName, PoolType = "Rechthoekig", Length = 12, Width = 6, Depth = 1.8m, NumberOfLights = 6, HasStairs = false, Status = "Done", ProposedPrice = 18000, RequestDate = DateTime.Now.AddDays(-30), ResponseDate = DateTime.Now.AddDays(-25) },
                new QuoteRequest { CustomerId = customerA.Id, CustomerName = customerA.UserName, PoolType = "Rechthoekig", Length = 15, Width = 8, Depth = 2m, NumberOfLights = 8, HasStairs = true, Status = "New", RequestDate = DateTime.Now.AddDays(-1) },

                // CustomerB: 2 completed + 1 open
                new QuoteRequest { CustomerId = customerB.Id, CustomerName = customerB.UserName, PoolType = "Ovaal", Length = 6, Width = 3, Depth = 1m, NumberOfLights = 2, HasStairs = true, Status = "Done", ProposedPrice = 9000, RequestDate = DateTime.Now.AddDays(-15), ResponseDate = DateTime.Now.AddDays(-10) },
                new QuoteRequest { CustomerId = customerB.Id, CustomerName = customerB.UserName, PoolType = "Rechthoekig", Length = 7, Width = 4, Depth = 1.2m, NumberOfLights = 3, HasStairs = false, Status = "Done", ProposedPrice = 11000, RequestDate = DateTime.Now.AddDays(-25), ResponseDate = DateTime.Now.AddDays(-20) },
                new QuoteRequest { CustomerId = customerB.Id, CustomerName = customerB.UserName, PoolType = "Rechthoekig", Length = 9, Width = 5, Depth = 1.5m, NumberOfLights = 4, HasStairs = true, Status = "In Progress", RequestDate = DateTime.Now.AddDays(-2) },

                // CustomerC: 1 completed + 0 open
                new QuoteRequest { CustomerId = customerC.Id, CustomerName = customerC.UserName, PoolType = "Rechthoekig", Length = 20, Width = 10, Depth = 2.5m, NumberOfLights = 10, HasStairs = true, Status = "Done", ProposedPrice = 35000, RequestDate = DateTime.Now.AddDays(-40), ResponseDate = DateTime.Now.AddDays(-35) }
            };

            context.QuoteRequests.AddRange(quoteRequests);
            context.SaveChanges();
        }
    }
}