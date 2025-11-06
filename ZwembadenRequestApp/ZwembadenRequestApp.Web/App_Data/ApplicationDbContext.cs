using System.Data.Entity;
using ZwembadenRequestApp.Core.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using ZwembadenRequestApp.Web.Models;

namespace ZwembadenRequestApp.Web.App_Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<QuoteRequest> QuoteRequests { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}