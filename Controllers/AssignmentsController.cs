using AphidBT.Helpers;
using AphidBT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AphidBT.Controllers
{
    public class AssignmentsController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectHelper projectHelper = new ProjectHelper();

        #region Role Assignments
        // GET: Assignments
        [Authorize(Roles = "Admin")]
        public ActionResult ManageRoles()
        {
            // Use my ViewBag to hold a multi select list of Users in the system
            // new MultiSelectList(the data itself, "Id", "Email")
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");

            // use my ViewBag to hold a select list of Roles
            ViewBag.RoleName = new SelectList(db.Roles.Where(r => r.Name != "Admin"), "Name", "Name");

            return View(db.Users.ToList());
        }

        public ActionResult ManageUserRoles()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(List<string> userIds, string roleName)
        {
            // Step 1: If anyone was selected, remove them from all their roles.
            if (userIds == null)
            {
                // there's nothing to do
                return RedirectToAction("RolesIndex");
            }

            // If people were selected, spin through them and strip them of all their current roles.
            foreach (var userId in userIds)
            {
                // determine if this user occupies a role
                foreach (var role in roleHelper.ListUserRoles(userId).ToList())
                {
                    roleHelper.RemoveUserFromRole(userId, role);
                }

                // Step 2: If a role was chosen, add each person to that role.
                if (!string.IsNullOrEmpty(roleName))
                {
                    roleHelper.AddUserToRole(userId, roleName);
                }
            }

            return RedirectToAction("ManageRoles");
        }
        #endregion


        #region Projects Assignments
        // Jason's versions from live presentation
        public ActionResult ManageProjectUsers()
        {
            // I want two list boxes in my view, therefore I want to load up 2 MultiSelectLists
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");

            // here, I will load up my projects list box
            ViewBag.ProjectIds = new MultiSelectList(db.Projects, "Id", "Name");

            return View(db.Users.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjectUsers(List<string> userIds, List<int> projectIds, bool remove)
        {
            // if no users, or no projects, send them back
            if (userIds == null || projectIds == null)
            {
                // nothing to do
                return RedirectToAction("ManageProjectUsers");
            }

            // iterate over each user and add/remove them to/from each project
            foreach (var userId in userIds)
            {
                // Assign/remove this person to each of the projects
                foreach (var projectId in projectIds)
                {
                    if (remove)
                    {
                        projectHelper.RemoveUserFromProject(userId, projectId);
                    }
                    else
                    {
                        projectHelper.AddUserToProject(userId, projectId);
                    }
                }
            }

            return RedirectToAction("ManageProjectUsers");
        }

        // END Jason's versions
        #endregion

    }
}