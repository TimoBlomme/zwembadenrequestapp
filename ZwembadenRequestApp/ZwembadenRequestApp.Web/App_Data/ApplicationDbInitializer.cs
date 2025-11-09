using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ZwembadenRequestApp.Core.Entities;
using ZwembadenRequestApp.Web.Models;

namespace ZwembadenRequestApp.Web.App_Data
{
    public class ApplicationDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        private void InitializeIdentityForEF(ApplicationDbContext context)
        {
            try
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                const string adminRole = "Admin";
                const string customerRole = "Customer";
                const string password = "Test123?";

                if (!roleManager.RoleExists(adminRole))
                {
                    roleManager.Create(new IdentityRole(adminRole));
                }
                if (!roleManager.RoleExists(customerRole))
                {
                    roleManager.Create(new IdentityRole(customerRole));
                }

                var adminUser = userManager.FindByName("admin");
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = "admin@pri.be",
                        EmailConfirmed = true,
                        FirstName = "Admin",
                        LastName = "User"
                    };
                    userManager.Create(adminUser, password);
                    userManager.AddToRole(adminUser.Id, adminRole);
                }

                CreateCustomerUser(userManager, "customerA", "customerA@pri.be", "Customer", "A", customerRole, password);
                CreateCustomerUser(userManager, "customerB", "customerB@pri.be", "Customer", "B", customerRole, password);
                CreateCustomerUser(userManager, "customerC", "customerC@pri.be", "Customer", "C", customerRole, password);

                context.SaveChanges();

                SeedQuoteRequests(context);

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Database seeding failed: " + ex.Message);
            }
        }

        private void CreateCustomerUser(UserManager<ApplicationUser> userManager, string userName, string email, string firstName, string lastName, string role, string password)
        {
            var user = userManager.FindByName(userName);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = userName,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = firstName,
                    LastName = lastName
                };
                userManager.Create(user, password);
                userManager.AddToRole(user.Id, role);
            }
        }

        private void SeedQuoteRequests(ApplicationDbContext context)
        {
            var customerA = context.Users.Find("customerA");
            var customerB = context.Users.Find("customerB");
            var customerC = context.Users.Find("customerC");

            if (customerA != null)
            {
                // CustomerA: 3 done quotes + 1 open quote
                context.QuoteRequests.Add(new QuoteRequest
                {
                    CustomerId = customerA.Id,
                    CustomerName = customerA.UserName,
                    PoolType = "Rechthoekig",
                    Length = 10,
                    Width = 5,
                    Depth = 2,
                    NumberOfLights = 2,
                    HasStairs = true,
                    Status = QuoteStatus.Done,
                    ProposedPrice = 25000,
                    RequestDate = DateTime.Now.AddDays(-30),
                    ResponseDate = DateTime.Now.AddDays(-25)
                });

                context.QuoteRequests.Add(new QuoteRequest
                {
                    CustomerId = customerA.Id,
                    CustomerName = customerA.UserName,
                    PoolType = "Ovaal",
                    Length = 8,
                    Width = 4,
                    Depth = 1.5m,
                    NumberOfLights = 1,
                    HasStairs = false,
                    Status = QuoteStatus.Done,
                    ProposedPrice = 18000,
                    RequestDate = DateTime.Now.AddDays(-20),
                    ResponseDate = DateTime.Now.AddDays(-15)
                });

                context.QuoteRequests.Add(new QuoteRequest
                {
                    CustomerId = customerA.Id,
                    CustomerName = customerA.UserName,
                    PoolType = "Rond",
                    Length = 6,
                    Width = 6,
                    Depth = 1.2m,
                    NumberOfLights = 0,
                    HasStairs = true,
                    Status = QuoteStatus.Done,
                    ProposedPrice = 12000,
                    RequestDate = DateTime.Now.AddDays(-10),
                    ResponseDate = DateTime.Now.AddDays(-5)
                });

                context.QuoteRequests.Add(new QuoteRequest
                {
                    CustomerId = customerA.Id,
                    CustomerName = customerA.UserName,
                    PoolType = "Vrij Vorm",
                    Length = 12,
                    Width = 8,
                    Depth = 2.5m,
                    NumberOfLights = 3,
                    HasStairs = true,
                    Status = QuoteStatus.New,
                    ProposedPrice = null,
                    RequestDate = DateTime.Now.AddDays(-2),
                    ResponseDate = null
                });
            }

            if (customerB != null)
            {
                // CustomerB: 2 done quotes + 1 open quote
                context.QuoteRequests.Add(new QuoteRequest
                {
                    CustomerId = customerB.Id,
                    CustomerName = customerB.UserName,
                    PoolType = "Rechthoekig",
                    Length = 15,
                    Width = 10,
                    Depth = 3,
                    NumberOfLights = 8,
                    HasStairs = true,
                    Status = QuoteStatus.Done,
                    ProposedPrice = 50000,
                    RequestDate = DateTime.Now.AddDays(-30),
                    ResponseDate = DateTime.Now.AddDays(-25)
                });

                context.QuoteRequests.Add(new QuoteRequest
                {
                    CustomerId = customerB.Id,
                    CustomerName = customerB.UserName,
                    PoolType = "Rond",
                    Length = 6,
                    Width = 6,
                    Depth = 1.2m,
                    NumberOfLights = 0,
                    HasStairs = true,
                    Status = QuoteStatus.Done,
                    ProposedPrice = 12000,
                    RequestDate = DateTime.Now.AddDays(-10),
                    ResponseDate = DateTime.Now.AddDays(-5)
                });

                context.QuoteRequests.Add(new QuoteRequest
                {
                    CustomerId = customerB.Id,
                    CustomerName = customerB.UserName,
                    PoolType = "Vrij Vorm",
                    Length = 12,
                    Width = 8,
                    Depth = 2.5m,
                    NumberOfLights = 3,
                    HasStairs = true,
                    Status = QuoteStatus.New,
                    ProposedPrice = null,
                    RequestDate = DateTime.Now.AddDays(-2),
                    ResponseDate = null
                });
            }

            if (customerC != null)
            {
                // CustomerC: 1 done quotes
                context.QuoteRequests.Add(new QuoteRequest
                {
                    CustomerId = customerC.Id,
                    CustomerName = customerC.UserName,
                    PoolType = "Rechthoekig",
                    Length = 15,
                    Width = 10,
                    Depth = 3,
                    NumberOfLights = 8,
                    HasStairs = true,
                    Status = QuoteStatus.Done,
                    ProposedPrice = 50000,
                    RequestDate = DateTime.Now.AddDays(-30),
                    ResponseDate = DateTime.Now.AddDays(-25)
                });
            }
        }
    }
}