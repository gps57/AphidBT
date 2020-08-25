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
            string demoEmail = "";
            var demoPassword = WebConfigurationManager.AppSettings["DemoPassword"];

            //  Admin role
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            //  Project Manager role
            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }

            // Developer role
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            // Submitter role
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            // Seed some users with roles
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            SeedHelper seedHelper = new SeedHelper();
            seedHelper.SeedDemoUsers();

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
            }

            // Seed a couple generic users
            if (!context.Users.Any(u => u.Email == "user1@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "user1@mailinator.com",
                    UserName = "user1@mailinator.com",
                    FirstName = "User",
                    LastName = "One"
                }, "Abc&123!");

                // grab the Id that just created by adding the above user
                var userId = userManager.FindByEmail("user1@mailinator.com").Id;

                // now that I have created a user I want to assign the person to the specific role
                userManager.AddToRole(userId, "Submitter");
            }

            if (!context.Users.Any(u => u.Email == "user2@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "user2@mailinator.com",
                    UserName = "user2@mailinator.com",
                    FirstName = "User",
                    LastName = "Two"
                }, "Abc&123!");

                // grab the Id that just created by adding the above user
                var userId = userManager.FindByEmail("user2@mailinator.com").Id;

                // now that I have created a user I want to assign the person to the specific role
                userManager.AddToRole(userId, "Submitter");
            }


            context.SaveChanges();

            //seeding some ticket stuff
            #region TicketType Seed
            context.TicketTypes.AddOrUpdate(
                tt => tt.Name,
                new TicketType() { Name = "Software" },
                new TicketType() { Name = "Hardware" },
                new TicketType() { Name = "UI" },
                new TicketType() { Name = "Defect" },
                new TicketType() { Name = "Feature Request" },
                new TicketType() { Name = "Other" }
                );
            #endregion

            #region TicketPriority Seed
            context.TicketPriorities.AddOrUpdate(
                pp => pp.Name,
                new TicketPriority() { Name = "Low" },
                new TicketPriority() { Name = "Medium" },
                new TicketPriority() { Name = "High" },
                new TicketPriority() { Name = "On Hold" }
                );
            #endregion

            #region TicketStatus Seed
            context.TicketStatuses.AddOrUpdate(
                ts => ts.Name,
                new TicketStatus() { Name = "Open" },
                new TicketStatus() { Name = "Assigned" },
                new TicketStatus() { Name = "Resolved" },
                new TicketStatus() { Name = "Reopened" },
                new TicketStatus() { Name = "Archived" }
                );
            #endregion

            #region Project Seed
            context.Projects.AddOrUpdate(
                p => p.Name,
                new Project() { Name = "Seed 1", Created = DateTime.Now.AddDays(-60), IsArchived = true },
                new Project() { Name = "Seed 2", Created = DateTime.Now.AddDays(-45) },
                new Project() { Name = "Seed 3", Created = DateTime.Now.AddDays(-30) },
                new Project() { Name = "Seed 4", Created = DateTime.Now.AddDays(-15) },
                new Project() { Name = "Seed 5", Created = DateTime.Now.AddDays(-7) }
                );
            #endregion

            context.SaveChanges();

        }
    }
}
