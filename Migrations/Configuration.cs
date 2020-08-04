namespace AphidBT.Migrations
{
    using AphidBT.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

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
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

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

            // set myself up as administrator
            if (!context.Users.Any(u => u.Email == "glen@swiftbt.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "glen@swiftbt.com",
                    UserName = "glen@swiftbt.com",
                    FirstName = "Glen",
                    LastName = "Stewart"
                }, "Abc&123!");

                // grab the Id that just created by adding the above user
                var userId = userManager.FindByEmail("glen@swiftbt.com").Id;

                // now that I have created a user I want to assign the person to the specific role
                userManager.AddToRole(userId, "Admin");

            }

            // set up a "Demo" Project Manager
            if (!context.Users.Any(u => u.Email == "demo_pm@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "demo_pm@mailinator.com",
                    UserName = "demo_pm@mailinator.com",
                    FirstName = "Demo",
                    LastName = "ProjectManager"
                }, "Abc&123!");

                // grab the Id that just created by adding the above user
                var userId = userManager.FindByEmail("demo_pm@mailinator.com").Id;

                // now that I have created a user I want to assign the person to the specific role
                userManager.AddToRole(userId, "Project Manager");

            }

            // set up a "Demo" Developer
            if (!context.Users.Any(u => u.Email == "demo_dev@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "demo_dev@mailinator.com",
                    UserName = "demo_dev@mailinator.com",
                    FirstName = "Demo",
                    LastName = "Developer"
                }, "Abc&123!");

                // grab the Id that just created by adding the above user
                var userId = userManager.FindByEmail("demo_dev@mailinator.com").Id;

                // now that I have created a user I want to assign the person to the specific role
                userManager.AddToRole(userId, "Developer");
            }

            // set up a "Demo Submitter"
            if (!context.Users.Any(u => u.Email == "demo_sub@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "demo_sub@mailinator.com",
                    UserName = "demo_sub@mailinator.com",
                    FirstName = "Demo",
                    LastName = "Submitter"
                }, "Abc&123!");

                // grab the Id that just created by adding the above user
                var userId = userManager.FindByEmail("demo_sub@mailinator.com").Id;

                // now that I have created a user I want to assign the person to the specific role
                userManager.AddToRole(userId, "Submitter");
            }

        }
    }
}
