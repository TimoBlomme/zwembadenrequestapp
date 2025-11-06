using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace ZwembadenRequestApp.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Achternaam")]
        public string LastName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public System.Data.Entity.DbSet<ZwembadenRequestApp.Core.Entities.QuoteRequest> QuoteRequests { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}