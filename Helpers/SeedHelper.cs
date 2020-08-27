using AphidBT.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace AphidBT.Helpers
{
    public class SeedHelper
    {

        private ApplicationDbContext db { get; set; }

        public SeedHelper(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        private Random random = new Random();
        private string demoPassword = WebConfigurationManager.AppSettings["DemoPassword"];
        private UserRolesHelper roleHelper = new UserRolesHelper();

        public void SeedRoles(RoleManager<IdentityRole> rm)
        {
            //  Admin role
            if (!db.Roles.Any(r => r.Name == "Admin"))
            {
                rm.Create(new IdentityRole { Name = "Admin" });
            }

            //  Project Manager role
            if (!db.Roles.Any(r => r.Name == "Project Manager"))
            {
                rm.Create(new IdentityRole { Name = "Project Manager" });
            }

            // Developer role
            if (!db.Roles.Any(r => r.Name == "Developer"))
            {
                rm.Create(new IdentityRole { Name = "Developer" });
            }

            // Submitter role
            if (!db.Roles.Any(r => r.Name == "Submitter"))
            {
                rm.Create(new IdentityRole { Name = "Submitter" });
            }

            db.SaveChanges();
        }

        public void SeedDemoUsers(UserManager<ApplicationUser> um)
        {
            string demoEmail = "";

            // set up a demo administrator
            demoEmail = WebConfigurationManager.AppSettings["AdminDemoEmail"];
            var avatar = WebConfigurationManager.AppSettings["DefaultAvatarPath"];

            if (!db.Users.Any(u => u.Email == demoEmail))
            {
                um.Create(new ApplicationUser()
                {
                    Email = demoEmail,
                    UserName = demoEmail,
                    FirstName = "Demo",
                    LastName = "Admin",
                    AvatarPath = avatar
                }, demoPassword);

                // grab the Id that just created by adding the above user
                var userId = um.FindByEmail(demoEmail).Id;

                // now that I have created a user I want to assign the person to the specific role
                um.AddToRole(userId, "Admin");
            }

            // set up a demo project manager
            demoEmail = WebConfigurationManager.AppSettings["ProjectManagerDemoEmail"];
            if (!db.Users.Any(u => u.Email == demoEmail))
            {
                um.Create(new ApplicationUser()
                {
                    Email = demoEmail,
                    UserName = demoEmail,
                    FirstName = "Demo",
                    LastName = "ProjectManager",
                    AvatarPath = avatar
                }, demoPassword);

                // grab the Id that just created by adding the above user
                var userId = um.FindByEmail(demoEmail).Id;

                // now that I have created a user I want to assign the person to the specific role
                um.AddToRole(userId, "Project Manager");
            }

            // set up a demo developer
            demoEmail = WebConfigurationManager.AppSettings["DeveloperDemoEmail"];
            if (!db.Users.Any(u => u.Email == demoEmail))
            {
                um.Create(new ApplicationUser()
                {
                    Email = demoEmail,
                    UserName = demoEmail,
                    FirstName = "Demo",
                    LastName = "Developer",
                    AvatarPath = avatar
                }, demoPassword);

                // grab the Id that just created by adding the above user
                var userId = um.FindByEmail(demoEmail).Id;

                // now that I have created a user I want to assign the person to the specific role
                um.AddToRole(userId, "Developer");
            }

            // set up a demo submitter
            demoEmail = WebConfigurationManager.AppSettings["SubmitterDemoEmail"];
            if (!db.Users.Any(u => u.Email == demoEmail))
            {
                um.Create(new ApplicationUser()
                {
                    Email = demoEmail,
                    UserName = demoEmail,
                    FirstName = "Demo",
                    LastName = "Submitter",
                    AvatarPath = avatar
                }, demoPassword);

                // grab the Id that just created by adding the above user
                var userId = um.FindByEmail(demoEmail).Id;

                // now that I have created a user I want to assign the person to the specific role
                um.AddToRole(userId, "Submitter");
            }

            db.SaveChanges();
        }

        public void SeedRamdomUsers(UserManager<ApplicationUser> um)
        {
            var avatar = WebConfigurationManager.AppSettings["DefaultAvatarPath"];

            List<string> firstNames = new List<string>()
            {
                "Bonnie",
                "Eric",
                "John",
                "Mollie",
                "Nancy",
                "Jimi"
            };

            List<string> lastNames = new List<string>()
            {
                "Raitt",
                "Clapton",
                "Mayer",
                "Tuttle",
                "Wilson",
                "Hendrix"
            };

            var roles = db.Roles.ToList();

            for (var i = 0; i < 10; i++)
            {
                var fName = firstNames[random.Next(firstNames.Count)];
                var lName = lastNames[random.Next(lastNames.Count)];
                var uName = $"{fName}.{lName}@mailinator.com";

                if (!db.Users.Any(u => u.Email == uName))
                {
                    um.Create(new ApplicationUser
                    {
                        UserName = uName,
                        Email = uName,
                        FirstName = fName,
                        LastName = lName,
                        AvatarPath = avatar
                    }, demoPassword);

                    var userId = um.FindByEmail(uName).Id;
                    um.AddToRole(userId, roles[random.Next(roles.Count)].Name);
                }
            }

            db.SaveChanges();
        }

        public void SeedTicketTypes()
        {
            db.TicketTypes.AddOrUpdate(
                tt => tt.Name,
                new TicketType() { Name = "Software" },
                new TicketType() { Name = "UI" },
                new TicketType() { Name = "Defect" },
                new TicketType() { Name = "Invalid Data" },
                new TicketType() { Name = "Feature Request" },
                new TicketType() { Name = "Other" }
                );

            db.SaveChanges();
        }

        public void SeedTicketPriorities()
        {
            db.TicketPriorities.AddOrUpdate(
                pp => pp.Name,
                new TicketPriority() { Name = "Low" },
                new TicketPriority() { Name = "Medium" },
                new TicketPriority() { Name = "High" },
                new TicketPriority() { Name = "On Hold" }
                );

            db.SaveChanges();
        }

        public void SeedTicketStatuses()
        {
            db.TicketStatuses.AddOrUpdate(
                ts => ts.Name,
                new TicketStatus() { Name = "Open" },
                new TicketStatus() { Name = "Assigned" },
                new TicketStatus() { Name = "Resolved" },
                new TicketStatus() { Name = "Reopened" },
                new TicketStatus() { Name = "Archived" }
                );

            db.SaveChanges();
        }

        public void SeedDemoProjects()
        {


            db.Projects.AddOrUpdate(
                p => p.Name,
                new Project() { Name = "Project Seed 1", Created = DateTime.Now.AddDays(-60), IsArchived = true },
                new Project() { Name = "Project Seed 2", Created = DateTime.Now.AddDays(-45) },
                new Project() { Name = "Project Seed 3", Created = DateTime.Now.AddDays(-30) },
                new Project() { Name = "Project Seed 4", Created = DateTime.Now.AddDays(-15) },
                new Project() { Name = "Project Seed 5", Created = DateTime.Now.AddDays(-7) }
                );

            db.SaveChanges();

            // Assign users to projects
            ProjectHelper projectHelper = new ProjectHelper();
            List<ApplicationUser> projectManagers = new List<ApplicationUser>();
            List<ApplicationUser> projectDevelopers = new List<ApplicationUser>();
            List<ApplicationUser> projectSubmitters = new List<ApplicationUser>();
            projectManagers.AddRange(roleHelper.UsersInRole("Project Manager"));
            projectDevelopers.AddRange(roleHelper.UsersInRole("Developer"));
            projectSubmitters.AddRange(roleHelper.UsersInRole("Submitter"));
            foreach (var project in db.Projects)
            {
                // assign all "Admin" users as project managers for all projects
                foreach (var user in roleHelper.UsersInRole("Admin"))
                {
                    projectHelper.AddUserToProject(user.Id, project.Id);
                }

                // randomly assign another project manager to each project
                projectHelper.AddUserToProject
                (
                    projectManagers[random.Next(projectManagers.Count)].Id,
                    project.Id
                );

                // randomly assign 2 developers to each project
                var firstDev = projectDevelopers[random.Next(projectDevelopers.Count)].Id;
                projectHelper.AddUserToProject(firstDev, project.Id);

                // seed the second developer only if there is more than one available
                if (projectDevelopers.Count > 1)
                {
                    var secondDev = projectDevelopers[random.Next(projectDevelopers.Count)].Id;

                    while (firstDev == secondDev)
                    {
                        secondDev = projectDevelopers[random.Next(projectDevelopers.Count)].Id;
                    }

                    projectHelper.AddUserToProject(secondDev, project.Id);
                }

                // randomly assign 2 submitters to each project
                var firstSub = projectSubmitters[random.Next(projectSubmitters.Count)].Id;
                projectHelper.AddUserToProject(firstSub, project.Id);

                // seed the second submitter only if there is more than one available
                if (projectSubmitters.Count > 1)
                {
                    var secondSub = projectSubmitters[random.Next(projectSubmitters.Count)].Id;

                    while (firstSub == secondSub)
                    {
                        secondSub = projectSubmitters[random.Next(projectSubmitters.Count)].Id;
                    }

                    projectHelper.AddUserToProject(secondSub, project.Id);
                }
            }
        }

        public void SeedDemoTickets()
        {
            ProjectHelper projectHelper = new ProjectHelper();
            var projects = db.Projects.ToList();
            var ticketPriorities = db.TicketPriorities.ToList();
            var ticketTypes = db.TicketTypes.ToList();
            var ticketStatuses = db.TicketStatuses.ToList();

            for (var i = 0; i < 100; i++)
            {
                var aProject = projects[random.Next(projects.Count())];
                var projectSubmitters = projectHelper.ListUsersOnProjectInRole(aProject.Id, "Submitter");
                var projectDevelopers = projectHelper.ListUsersOnProjectInRole(aProject.Id, "Developer");
                var ticketStatus = ticketStatuses[random.Next(ticketStatuses.Count())];

                var developerId = "";
                if (projectDevelopers.Count != 0)
                {
                    developerId = projectDevelopers[random.Next(projectDevelopers.Count)].Id;
                }

                if (ticketStatus.Name == "Open")
                {
                    developerId = null;
                }

                var resolved = false;
                if(ticketStatus.Name == "Resolved")
                {
                    resolved = true;
                }

                var archived = false;
                if(ticketStatus.Name == "Archived" || aProject.IsArchived)
                {
                    archived = true;
                }

                var newTicket = new Ticket()
                {
                    Projectid = aProject.Id,
                    TicketPriorityId = ticketPriorities[random.Next(ticketPriorities.Count())].Id,
                    TicketTypeId = ticketTypes[random.Next(ticketTypes.Count())].Id,
                    TicketStatusId = ticketStatus.Id,
                    SubmitterId = projectSubmitters[random.Next(projectSubmitters.Count)].Id,
                    DeveloperId = developerId,
                    Created = DateTime.Now.AddDays(-2),
                    Updated = DateTime.Now,
                    Title = $"Seeded Ticket {i}",
                    Description = "This is a description of Ticket. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris nulla ligula, vehicula sed lectus ac, volutpat rutrum sem. Nunc purus tortor, faucibus tincidunt rhoncus vel, porta in metus. Proin pulvinar.",
                    IsResolved = resolved,
                    IsArchived = archived
                };

                db.Tickets.Add(newTicket);
            }

            db.SaveChanges();
        }

        public void SeedHistories()
        {
            List<ApplicationUser> projectManagers = new List<ApplicationUser>();
            projectManagers.AddRange(roleHelper.UsersInRole("Project Manager"));

            List<ApplicationUser> projectDevelopers = new List<ApplicationUser>();
            projectDevelopers.AddRange(roleHelper.UsersInRole("Developer"));

            var oldDeveloper = projectDevelopers[random.Next(projectDevelopers.Count)];
            var newDeveloper = projectDevelopers[random.Next(projectDevelopers.Count)];
            if(projectDevelopers.Count > 1)
            {
                while(oldDeveloper.Id == newDeveloper.Id)
                {
                    newDeveloper = projectDevelopers[random.Next(projectDevelopers.Count)];
                }
            }

            foreach (var ticket in db.Tickets)
            {
                for(var i=0; i<5; i++)
                {
                    var history = new TicketHistory()
                    {
                        TicketId = ticket.Id,
                        UserId = projectManagers[random.Next(projectManagers.Count)].Id,
                        Property = "Developer",
                        OldValue = oldDeveloper.FullName,
                        NewValue = newDeveloper.FullName,
                        ChangedOn = DateTime.Now.AddDays(-i)
                    };

                    db.TicketHistories.Add(history);
                }
            }
            db.SaveChanges();
        }
    }
}