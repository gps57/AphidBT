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


        public bool CanEditTicket(int ticketId)
        {
            // same code as CanMakeComment().
            // TODO: Confirm that the rules for who can make comments are the same as for who can edit a ticket
            // shouldn't the code be extracted to a private method and use by both CanEditTicket() and
            // CanMakeComment()
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            var user = db.Users.Find(userId);

            switch (myRole)
            {
                case "Admin":
                    return true;

                case "Project Manager":
                    // project manager can make a comment if the ticket is in a project to which they are assigned.
                    return user.Projects.SelectMany(p => p.Tickets).Any(t => t.Id == ticketId);

                case "Developer":
                case "Submitter":
                    // developer can make a comment if the ticket is in a project to which they are assigned.
                    var ticket = db.Tickets.Find(ticketId);
                    if (ticket.DeveloperId == userId || ticket.SubmitterId == userId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                default:
                    return false;
            }
        }

        public bool CanMakeComment(int ticketId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();            
            var user = db.Users.Find(userId);

            switch (myRole)
            {
                case "Admin":
                    return true;

                case "Project Manager":
                    // project manager can make a comment if the ticket is in a project to which they are assigned.
                    return user.Projects.SelectMany(p => p.Tickets).Any(t => t.Id == ticketId);

                case "Developer":
                case "Submitter":
                    // developer can make a comment if the ticket is in a project to which they are assigned.
                    var ticket = db.Tickets.Find(ticketId);
                    if (ticket.DeveloperId == userId || ticket.SubmitterId == userId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                default:
                    return false;
            }
        }

        public int TicketCount()
        {
            return db.Tickets.ToList().Count;
        }

        public int ResolvedTicketsCount()
        {
            int rCount = 0;

            foreach (var rt in db.Tickets.ToList())
            {
                if (rt.IsResolved)
                {
                    rCount++;
                }
            }
            return rCount;
        }

        public int ArchivedTicketsCount()
        {
            int aCount = 0;

            foreach (var at in db.Tickets.ToList())
            {
                if (at.IsArchived)
                {
                    aCount++;
                }
            }
            return aCount;
        }

        public void ManageTicketNotifications(Ticket oldTicket, Ticket newTicket)
        {
            // scenario 1: a new assignment (oldTicket.DeveloperId is null, newTicket.DeveloperId is not)

            // these next two scenarios are not technically required according to the SRS
            // scenario 2: unassignment (oldTicket.DeveloperId is not null, newTicket.DeveloperId is null)

            // scenario 3: reassignment (neither are null, but they are different)
            // scenario 3 would result in 2 notifications, to new developer, and to old developer.

            // according to the deliverables, all I need to do is scenario 1
            if (oldTicket.DeveloperId != newTicket.DeveloperId && newTicket.DeveloperId != null)
            {
                // create a new TicketNotification record
                var notification = new TicketNotification()
                {
                    UserId = newTicket.DeveloperId,
                    Created = DateTime.Now,
                    TicketId = newTicket.Id,
                    // Jason has a subject in his TicketNotification model
                    Message = $"Heads up {newTicket.Developer.FirstName}, you have been assigned to TicketId {newTicket.Title}"
                };

                db.TicketNotifications.Add(notification);
                db.SaveChanges();
                // TODO: was not able to keep up.  I'll have to watch the video to see what I missed.
            }
        }

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