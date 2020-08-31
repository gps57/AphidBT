using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AphidBT.Models;
using AphidBT.Helpers;
using Microsoft.AspNet.Identity;


namespace AphidBT.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectHelper projectHelper = new ProjectHelper();

        // GET: Projects
        [Authorize]
        public ActionResult Index()
        {
            // get the user who is logged in so I can show only the projects they are assigned to
            var loggedInUserId = User.Identity.GetUserId();

            // if this user is the admin, return all projects
            if (roleHelper.IsUserInRole(loggedInUserId, "Admin") || roleHelper.IsUserInRole(loggedInUserId, "Project Manager"))
            {
                return View(db.Projects.ToList());
            }
            else
            {
                var resultList = new List<Project>();

                foreach (var project in db.Projects)
                {
                    if (projectHelper.IsUserOnProject(loggedInUserId, project.Id))
                    {
                        resultList.Add(project);
                    }

                    //foreach (var user in project.Users.ToList())
                    //{
                    //    if (user.Id == loggedInUserId)
                    //    {

                    //    }
                    //}
                }
                return View(resultList);
            }

            // this is the original return statement
            // if things go wrong, revert back to this.
            //return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");

            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin,  Project Manager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.Created = DateTime.Now;
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Dashboard", "Projects", new { Id = project.Id });
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Projects/Edit/5
        public ActionResult Dashboard(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET:
        public ActionResult EditProject(int id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProject(int Id, string name)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(project).State = EntityState.Modified;
                db.Projects.Find(Id).Name = name;
                db.SaveChanges();
                ViewBag.projectedit = "You Project Name has been updated.";
            }
            else
            {
                ViewBag.projectedit = "Something went wrong.  Your project name was not updated.";
            }

            return RedirectToAction("Index", "Projects");
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Created,IsArchived")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public ActionResult AssignProjectManagers(List<string> managerIds, int projectId)
        {
            // if managerIds is empty, we still want to remove all the users from this project,
            // because that could be the user's intent
            foreach (var userId in projectHelper.ListUserIdsOnProjectInRole(projectId, "Project Manager"))
            {
                projectHelper.RemoveUserFromProject(userId, projectId);
            }

            if (managerIds.Count == 0)
            {
                return RedirectToAction("Dashboard", "Projects", new { Id = projectId });
            }

            foreach(var Id in managerIds)
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    projectHelper.AddUserToProject(Id, projectId);
                }
            }

            return RedirectToAction("Dashboard", "Projects", new { Id = projectId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public ActionResult AssignProjectDevelopers(List<string> developerIds, int projectId)
        {
            foreach (var userId in projectHelper.ListUserIdsOnProjectInRole(projectId, "Developer"))
            {
                projectHelper.RemoveUserFromProject(userId, projectId);
            }

            foreach (var Id in developerIds)
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    projectHelper.AddUserToProject(Id, projectId);
                }
            }

            return RedirectToAction("Dashboard", "Projects", new { Id = projectId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public ActionResult AssignProjectSubmitters(List<string> submitterIds, int projectId)
        {
            foreach (var userId in projectHelper.ListUserIdsOnProjectInRole(projectId, "Submitter"))
            {
                projectHelper.RemoveUserFromProject(userId, projectId);
            }

            foreach (var Id in submitterIds)
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    projectHelper.AddUserToProject(Id, projectId);
                }
            }

            return RedirectToAction("Dashboard", "Projects", new { Id = projectId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public ActionResult UpdateProjectName(string projectName, int projectId)
        {
            db.Projects.Find(projectId).Name = projectName;
            db.SaveChanges();

            return RedirectToAction("Dashboard", "Projects", new { Id = projectId });
        }
    }
}
