using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AphidBT.Helpers;
using AphidBT.Models;
using AphidBT.ViewModels;
using Microsoft.AspNet.Identity;

namespace AphidBT.Controllers
{
    public class TicketsController : Controller
    {
        // uncomment this when you are done testing
        //[Authorize]

        private ApplicationDbContext db = new ApplicationDbContext();
        private TicketHelper ticketHelper = new TicketHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();
        private HistoryHelper historyHelper = new HistoryHelper();

        // GET: Tickets
        [Authorize]
        public ActionResult Index()
        {
            var ticketListVM = new TicketsListVM();
            ticketListVM.AllTickets = db.Tickets.ToList();
            ticketListVM.MyTickets = ticketHelper.GetMyTickets();

            return View(ticketListVM);
        }

        // GET: Tickets/Dashboard/5
        public ActionResult Dashboard(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        // uncomment this when done with testing
        //[Authorize(Roles = "Submitter")]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();

            // this check might not be necessary after testing is done because the [Authorize] check will
            // send them to the login page if they are not logged in before it does anything else.
            if (userId == null)
            {
                return RedirectToAction("Index");
            }

            // Drew's code didn't have the following line of code in it.  Don't know why.
            // I'll comment it out until I know for sure that I really don't need it.
            //ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FirstName");

            ViewBag.Projectid = new SelectList(projectHelper.ListUserProjects(userId), "Id", "Name");

            // Drew's code didn't have the following line of code in it.  Don't know why.
            // I'll comment it out until I know for sure that I really don't need it.
            //ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FirstName");

            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            //ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Submitter")]
        public ActionResult Create([Bind(Include = "Id,Projectid,TicketPriorityId,TicketTypeId,Title,Description")] Ticket ticket)
        {
            var userId = User.Identity.GetUserId();
            
            if (ModelState.IsValid)
            {
                // Set DeveloperId to null, IsArchived and IsResolved will be false
                ticket.Created = DateTime.Now;

                // set Status to open because when a submitter submits a ticket it is automatically in the open status
                ticket.TicketStatusId = db.TicketStatuses.Where(ts => ts.Name == "Open").FirstOrDefault().Id;

                // User.Identity.GetUserId() gives the user id of the currently logged in user
                ticket.SubmitterId = userId;
                db.Tickets.Add(ticket);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            //ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FirstName", ticket.DeveloperId);
            ViewBag.Projectid = new SelectList(projectHelper.ListUserProjects(userId), "Id", "Name");
            //ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FirstName", ticket.SubmitterId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            //ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FirstName", ticket.DeveloperId);
            ViewBag.Projectid = new SelectList(db.Projects, "Id", "Name", ticket.Projectid);
            ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FirstName", ticket.SubmitterId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Projectid,TicketPriorityId,TicketStatusId,TicketTypeId,SubmitterId,DeveloperId,Title,Description,Created,Updated,IsResolved,IsArchived")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FirstName", ticket.DeveloperId);
            ViewBag.Projectid = new SelectList(db.Projects, "Id", "Name", ticket.Projectid);
            ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FirstName", ticket.SubmitterId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> AssignDeveloper(string developerId, int ticketId)
        {
            // TODO: Add call to notificationHelper.ManageNotification()
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);

            var ticket = db.Tickets.Find(ticketId);
            if (string.IsNullOrEmpty(developerId))
            {
                ticket.TicketStatusId = db.TicketStatuses.Where(s => s.Name == "Open").FirstOrDefault().Id;
                ticket.DeveloperId = null;
            }
            else
            {
                ticket.TicketStatusId = db.TicketStatuses.Where(s => s.Name == "Assigned").FirstOrDefault().Id;
                ticket.DeveloperId = developerId;
            }
            
            db.SaveChanges();

            var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);
            notificationHelper.ManageTicketNotifications(oldTicket, newTicket);
            historyHelper.DeveloperUpdate(oldTicket, newTicket);

            return RedirectToAction("Dashboard", "Tickets", new { Id = ticketId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public ActionResult UpdateTicketDescription(string ticketDescription, int ticketId)
        {
            db.Tickets.Find(ticketId).Description = ticketDescription;
            db.SaveChanges();
            return RedirectToAction("Dashboard", "Tickets", new { Id = ticketId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public ActionResult UpdateTicketTitle(string ticketTitle, int ticketId)
        {
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);
            db.Tickets.Find(ticketId).Title = ticketTitle;
            db.SaveChanges();
            var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);
            historyHelper.TitleUpdate(oldTicket, newTicket);

            return RedirectToAction("Dashboard", "Tickets", new { Id = ticketId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public ActionResult UpdateTicketPriority(int ticketPriorityId, int ticketId)
        {
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);
            db.Tickets.Find(ticketId).TicketPriorityId = ticketPriorityId;
            db.SaveChanges();
            var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);
            historyHelper.PriorityUpdate(oldTicket, newTicket);

            return RedirectToAction("Dashboard", "Tickets", new { Id = ticketId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public ActionResult UpdateTicketStatus(int ticketStatusId, int ticketId)
        {
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);
            db.Tickets.Find(ticketId).TicketStatusId = ticketStatusId;
            db.SaveChanges();
            var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);
            historyHelper.StatusUpdate(oldTicket, newTicket);
            return RedirectToAction("Dashboard", "Tickets", new { Id = ticketId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public ActionResult UpdateTicketType(int ticketTypeId, int ticketId)
        {
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);
            db.Tickets.Find(ticketId).TicketTypeId = ticketTypeId;
            db.SaveChanges();
            var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);
            historyHelper.TypeUpdate(oldTicket, newTicket);
            return RedirectToAction("Dashboard", "Tickets", new { Id = ticketId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public ActionResult SetNotifyIsRead(int Id, bool isRead)
        {
            db.TicketNotifications.Find(Id).IsRead = isRead;
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
