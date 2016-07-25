using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using WebApi2_Owin_BasicAuthentication.AuthenticationServer.Models;

namespace WebApi2_Owin_BasicAuthentication.AuthenticationServer.DataAccesLayer
{
    /// <summary>
    /// Class responsible for connecting with the database
    /// </summary>
    public class ApplicationUserContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationUserContext() : base("DefaultConnection") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("AppUser");
        }

    }
}
