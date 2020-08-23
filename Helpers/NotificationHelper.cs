using AphidBT.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace AphidBT.Helpers
{
    public class NotificationHelper
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private TicketHelper ticketHelper = new TicketHelper();

        public void ManageTicketNotifications(Ticket oldTicket, Ticket newTicket)
        {
            // these next two scenarios are not technically required according to the SRS
            // scenario 2: unassignment (oldTicket.DeveloperId is not null, newTicket.DeveloperId is null)

            // scenario 3: reassignment (neither are null, but they are different)
            // scenario 3 would result in 2 notifications, to new developer, and to old developer.

            // scenario 1: a new assignment (oldTicket.DeveloperId is null, newTicket.DeveloperId is not)
            // according to the deliverables, all I need to do is scenario 1
            if (oldTicket.DeveloperId != newTicket.DeveloperId && !string.IsNullOrEmpty(newTicket.DeveloperId))
            {
                // create a new TicketNotification record to notify the new developer of assignment
                var notification = new TicketNotification()
                {
                    UserId = newTicket.DeveloperId,
                    Created = DateTime.Now,
                    TicketId = newTicket.Id,
                    // Jason has a subject in his TicketNotification model
                    Subject = "Ticket Assignment",
                    Message = $"{newTicket.Developer.FirstName} has been assigned to Ticket Id {newTicket.Id},'{newTicket.Title}' on Project '{newTicket.Project.Name}'"
                };

                db.TicketNotifications.Add(notification);
                db.SaveChanges();

                // send the notification to the user's email
                // commented out because I'm getting an error that I think has to do with async and await
                // SendNotificationEmail(notification);
            }

            if((oldTicket.DeveloperId != newTicket.DeveloperId) && !string.IsNullOrEmpty(oldTicket.DeveloperId))
            {
                // create a new TicketNotification record to un-assign the old developer
                var notification = new TicketNotification()
                {
                    UserId = oldTicket.DeveloperId,
                    Created = DateTime.Now,
                    TicketId = oldTicket.Id,
                    Subject = "Ticket Un-Assignment",
                    Message = $"{oldTicket.Developer.FirstName} has been un-assigned from Ticket Id {oldTicket.Id},'{newTicket.Title}' on Project '{oldTicket.Project.Name}'"
                };

                db.TicketNotifications.Add(notification);
                db.SaveChanges();

                // send the notification to the user's email
                // commented out because I'm getting an error that I think has to do with async and await
                // SendNotificationEmail(notification);
            }
        }

        private async void SendNotificationEmail(TicketNotification notification)
        {
            try
            {
                var sendTo = notification.User.Email;
                var from = "MyPortfolio<example@email.com>";

                var email = new MailMessage(from, sendTo)
                {
                    Subject = notification.Subject,
                    Body = notification.Message,
                    IsBodyHtml = true
                };

                var svc = new EmailService();
                await svc.SendAsync(email);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }
        }

        public bool AnyUnreadNotifications()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            List<TicketNotification> ticketNotifications = db.TicketNotifications.Where(t => t.UserId == userId).ToList();
            // if there is one notification where IsRead is false, it means there are UnRead notifications, so
            // we return true.
            if (ticketNotifications.FirstOrDefault(tn => tn.IsRead == false) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}