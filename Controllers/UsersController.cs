using AphidBT.Helpers;
using AphidBT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AphidBT.Controllers
{


    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        public ActionResult ManageUserRole(string id)
        {
            // Does this user already occupy a Roll??  If so, I need to display that role in the dropdown
            var userRole = roleHelper.ListUserRoles(id).FirstOrDefault();

            ViewBag.RoleName = new SelectList(db.Roles, "Name", "Name", userRole);

            return View(db.Users.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUserRole(string id, string roleName)
        {
            // Code in here is very similar to code we have already seen
            // I need to remove all the roles from this user and add back the chose role
            // spin through all the roles for this user and remove them
            foreach(var role in roleHelper.ListUserRoles(id))
            {
                roleHelper.RemoveUserFromRole(id, role);
            }

            // if a role was chosen, then add this user to that role.
            if (!string.IsNullOrEmpty(roleName))
            {
                roleHelper.AddUserToRole(id, roleName);
            }

            // now that the user has been assigned a role I want to redirect them back to the page they came from
            return RedirectToAction("ManageUserRole", new { id });
        }
    }
}