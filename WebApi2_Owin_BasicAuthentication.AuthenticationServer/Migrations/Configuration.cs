namespace WebApi2_Owin_BasicAuthentication.AuthenticationServer.Migrations
{
    using DataAccesLayer;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApi2_Owin_BasicAuthentication.AuthenticationServer.DataAccesLayer.ApplicationUserContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebApi2_Owin_BasicAuthentication.AuthenticationServer.DataAccesLayer.ApplicationUserContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationUserContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationUserContext()));

            // Create some users
            var user = new ApplicationUser()
            {
                UserName = "SuperAdmin",
                Email = "poulj@whatever.ie",
                EmailConfirmed = true,
                FirstName = "Paul",
                LastName = "Jehova",
                Level = 1,
                JoinDate = DateTime.Now.AddYears(-3)
            };

            var user1 = new ApplicationUser()
            {
                UserName = "Phillipa",
                Email = "Phillipa@whatever.ie",
                EmailConfirmed = true,
                FirstName = "Phillip",
                LastName = "a",
                Level = 1,
                JoinDate = DateTime.Now.AddYears(-1)
            };

            manager.Create(user, "P@assword!");
            manager.Create(user1, "P@assword1!");


            // Create some roles
            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }


            // Assign roles
            var adminUser = manager.FindByName("SuperAdmin");
            manager.AddToRoles(adminUser.Id, new string[] { "SuperAdmin", "Admin" });

            var adminUser1 = manager.FindByName("Phillipa");
            manager.AddToRoles(adminUser1.Id, new string[] { "Admin", "User" });
        }
    }
}
