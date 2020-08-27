namespace AphidBT.Migrations
{
    using AphidBT.Helpers;
    using AphidBT.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<AphidBT.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AphidBT.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            // Create the roles that will be used in AphidBT
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var demoPassword = WebConfigurationManager.AppSettings["DemoPassword"];
            
            SeedHelper seedHelper = new SeedHelper(context);
            seedHelper.SeedRoles(roleManager);
            seedHelper.SeedDemoUsers(userManager);
            seedHelper.SeedRamdomUsers(userManager);
            seedHelper.SeedTicketTypes();
            seedHelper.SeedTicketPriorities();
            seedHelper.SeedTicketStatuses();
            seedHelper.SeedDemoProjects();
            seedHelper.SeedDemoTickets();

            // set myself up as administrator
            if (!context.Users.Any(u => u.Email == "glen@swiftbt.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "glen@swiftbt.com",
                    UserName = "glen@swiftbt.com",
                    FirstName = "Glen",
                    LastName = "Stewart"
                }, demoPassword);

                // grab the Id that just created by adding the above user
                var userId = userManager.FindByEmail("glen@swiftbt.com").Id;

                // now that I have created a user I want to assign the person to the specific role
                userManager.AddToRole(userId, "Admin");

                context.SaveChanges();
            }
        }
    }
}
