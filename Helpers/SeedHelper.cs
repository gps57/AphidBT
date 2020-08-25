using AphidBT.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace AphidBT.Helpers
{
    public class SeedHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void SeedDemoUsers()
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(db));
            var demoPassword = WebConfigurationManager.AppSettings["DemoPassword"];
            string demoEmail = "";

            // set up a demo administrator
            demoEmail = WebConfigurationManager.AppSettings["AdminDemoEmail"];
            if (!db.Users.Any(u => u.Email == demoEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = demoEmail,
                    UserName = demoEmail,
                    FirstName = "Demo",
                    LastName = "Admin"
                }, demoPassword);

                // grab the Id that just created by adding the above user
                var userId = userManager.FindByEmail(demoEmail).Id;

                // now that I have created a user I want to assign the person to the specific role
                userManager.AddToRole(userId, "Admin");
            }

            // set up a demo project manager
            demoEmail = WebConfigurationManager.AppSettings["ProjectManagerDemoEmail"];
            if (!db.Users.Any(u => u.Email == demoEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = demoEmail,
                    UserName = demoEmail,
                    FirstName = "Demo",
                    LastName = "ProjectManager"
                }, demoPassword);

                // grab the Id that just created by adding the above user
                var userId = userManager.FindByEmail(demoEmail).Id;

                // now that I have created a user I want to assign the person to the specific role
                userManager.AddToRole(userId, "Project Manager");
            }

            // set up a demo developer
            demoEmail = WebConfigurationManager.AppSettings["DeveloperDemoEmail"];
            if (!db.Users.Any(u => u.Email == demoEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = demoEmail,
                    UserName = demoEmail,
                    FirstName = "Demo",
                    LastName = "Developer"
                }, demoPassword);

                // grab the Id that just created by adding the above user
                var userId = userManager.FindByEmail(demoEmail).Id;

                // now that I have created a user I want to assign the person to the specific role
                userManager.AddToRole(userId, "Developer");
            }

            // set up a demo submitter
            demoEmail = WebConfigurationManager.AppSettings["SubmitterDemoEmail"];
            if (!db.Users.Any(u => u.Email == demoEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = demoEmail,
                    UserName = demoEmail,
                    FirstName = "Demo",
                    LastName = "Submitter"
                }, demoPassword);

                // grab the Id that just created by adding the above user
                var userId = userManager.FindByEmail(demoEmail).Id;

                // now that I have created a user I want to assign the person to the specific role
                userManager.AddToRole(userId, "Submitter");
            }
        }
    }
}