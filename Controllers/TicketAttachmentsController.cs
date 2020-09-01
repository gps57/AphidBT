using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using AphidBT.Helpers;
using AphidBT.Models;
using Microsoft.AspNet.Identity;

namespace AphidBT.Controllers
{
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketAttachments
        [Authorize]
        public ActionResult Index()
        {
            var ticketAttachments = db.TicketAttachments.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketAttachments.ToList());
        }

        [Authorize]
        // GET: TicketAttachments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "TicketId,FileName")] TicketAttachment ticketAttachment, string attachmentDescription, HttpPostedFileBase file)
        {
            // check that there is not an incoming file
            if (file == null)
            {
                TempData["Error"] = "You must supply a file.";
                return RedirectToAction("Dashboard", "Tickets", new { id = ticketAttachment.TicketId });
            }

            if (ModelState.IsValid)
            {
                if (FileUploadValidator.IsWebFriendlyImage(file) || FileUploadValidator.IsWebFriendlyFile(file))
                {
                    ticketAttachment.Description = attachmentDescription;
                    ticketAttachment.Created = DateTime.Now;
                    ticketAttachment.UserId = User.Identity.GetUserId();

                    var fileName = FileStamp.MakeUnique(file.FileName);

                    var serverFolder = WebConfigurationManager.AppSettings["DefaultAttachmentFolder"];
                    file.SaveAs(Path.Combine(Server.MapPath("~" + serverFolder), fileName));
                    ticketAttachment.FilePath = $"{serverFolder}/{fileName}";

                    db.TicketAttachments.Add(ticketAttachment);
                    db.SaveChanges();
                    
                }

                return RedirectToAction("Dashboard", "Tickets", new { id = ticketAttachment.TicketId });
            }

            TempData["Error"] = "The model was invalid for some reason!";
            return RedirectToAction("Dashboard", "Tickets", new { id = ticketAttachment.TicketId });

            //ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId", ticketAttachment.TicketId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            //return View(ticketAttachment);
        }

        // GET: TicketAttachments/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId", ticketAttachment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,TicketId,UserId,FilePath,Description,Created")] TicketAttachment ticketAttachment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketAttachment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId", ticketAttachment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            db.TicketAttachments.Remove(ticketAttachment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
