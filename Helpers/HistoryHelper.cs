using AphidBT.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AphidBT.Helpers
{
    public class HistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void ManageHistories(Ticket oldTicket, Ticket newTicket)
        {
            DeveloperUpdate(oldTicket, newTicket);
            TitleUpdate(oldTicket, newTicket);
            PriorityUpdate(oldTicket, newTicket);
            StatusUpdate(oldTicket, newTicket);
            TypeUpdate(oldTicket, newTicket);
        }

        public void DeveloperUpdate(Ticket oldTicket, Ticket newTicket)
        {
            if(oldTicket.DeveloperId != newTicket.DeveloperId)
            {
                var history = new TicketHistory()
                {
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    Property = "Developer",
                    OldValue = string.IsNullOrEmpty(oldTicket.DeveloperId) ? "No Developer Assigned" : oldTicket.Developer.FullName,
                    NewValue = string.IsNullOrEmpty(newTicket.DeveloperId) ? "No Developer Assigned" : newTicket.Developer.FullName,
                    ChangedOn = DateTime.Now
                };
                db.TicketHistories.Add(history);
                db.SaveChanges();
            }
        }

        public void TitleUpdate(Ticket oldTicket, Ticket newTicket)
        {
            if (oldTicket.Title != newTicket.Title)
            {
                var history = new TicketHistory()
                {
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    Property = "Title",
                    OldValue = string.IsNullOrEmpty(oldTicket.Title) ? "No Title" : oldTicket.Title,
                    NewValue = string.IsNullOrEmpty(newTicket.Title) ? "No Title" : newTicket.Title,
                    ChangedOn = DateTime.Now
                };
                db.TicketHistories.Add(history);
                db.SaveChanges();
            }
        }

        public void PriorityUpdate(Ticket oldTicket, Ticket newTicket)
        {
            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                var history = new TicketHistory()
                {
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    Property = "Priority",
                    OldValue = oldTicket.TicketPriority.Name,
                    NewValue = newTicket.TicketPriority.Name,
                    ChangedOn = DateTime.Now
                };
                db.TicketHistories.Add(history);
                db.SaveChanges();
            }
        }

        public void StatusUpdate(Ticket oldTicket, Ticket newTicket)
        {
            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                var history = new TicketHistory()
                {
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    Property = "Status",
                    OldValue = oldTicket.TicketStatus.Name,
                    NewValue = newTicket.TicketStatus.Name,
                    ChangedOn = DateTime.Now
                };
                db.TicketHistories.Add(history);
                db.SaveChanges();
            }

        }

        public void TypeUpdate(Ticket oldTicket, Ticket newTicket)
        {
            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                var history = new TicketHistory()
                {
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    Property = "Type",
                    OldValue = oldTicket.TicketType.Name,
                    NewValue = newTicket.TicketType.Name,
                    ChangedOn = DateTime.Now
                };
                db.TicketHistories.Add(history);
                db.SaveChanges();
            }
        }
    }
}