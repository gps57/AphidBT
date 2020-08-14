using AphidBT.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AphidBT.Helpers
{
    public class TicketHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        public List<Ticket> GetMyTickets()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            List<Ticket> tickets = new List<Ticket>();

            switch (myRole)
            {
                case "Admin":
                    tickets.AddRange(db.Tickets);
                    // drew's way
                    // tickets = db.Tickets.ToList();
                    break;

                case "Project Manager":
                    tickets.AddRange(db.Users.Find(userId).Projects.SelectMany(p => p.Tickets));
                    break;

                case "Developer":
                    tickets.AddRange(db.Tickets.Where(t => t.DeveloperId == userId));
                    // drew's way
                    // tickets = projectHelper.ListUserProjects(userId).SelectMany(p => p.Tickets).ToList();
                    break;

                case "Submitter":
                    tickets.AddRange(db.Tickets.Where(t => t.SubmitterId == userId));
                    // drew's way
                    // tickets = db.Tickets.Where(t => t.SubmitterId == userId).ToList();
                    break;

                // Drew went to the index home location, but I'm going to the Login Account page
                default:
                    // if you get here, the tickets list would be empty
                    break;
            }
            return tickets;
        }
    }
}