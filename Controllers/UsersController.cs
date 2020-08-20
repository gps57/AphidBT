using AphidBT.Helpers;
using AphidBT.Models;
using AphidBT.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace AphidBT.Controllers
{


    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
        private UserRolesHelper userRolesHelper = new UserRolesHelper();

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
            foreach (var role in roleHelper.ListUserRoles(id))
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

        // GET: User/Manage/x
        public ActionResult Manage(string userId)
        {
            var manageUserVM = new ManageUserVM();
            var user = db.Users.Find(userId);

            manageUserVM.UserId = userId;
            manageUserVM.FirstName = user.FirstName;
            manageUserVM.LastName = user.LastName;
            manageUserVM.Email = user.Email;
            manageUserVM.AvatarPath = user.AvatarPath;
            manageUserVM.Role = userRolesHelper.ListUserRoles(userId).FirstOrDefault();

            ViewBag.ProjectIds = new MultiSelectList(db.Projects, "Id", "Name", user.Projects.Select(p => p.Id));
            ViewBag.RoleName = new SelectList(db.Roles, "Name", "Name", manageUserVM.Role);

            return View(manageUserVM);
        }

        // POST: User/Details
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Submitter")]
        public ActionResult Manage(ManageUserVM model, string roleName, List<int> projectIds, HttpPostedFileBase avatar)
        {
            var user = db.Users.Find(model.UserId);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Email;

            if(avatar != null)
            {
                // TODO: When the avatar is changed, we'll have to delete the old file, or they'll just
                // pile up on the server.
                var serverFolder = WebConfigurationManager.AppSettings["DefaultServerFolder"];
                var oldAvatarFileName = Path.GetFileName(user.AvatarPath);

                if(System.IO.File.Exists(Path.Combine(serverFolder, oldAvatarFileName)))
                {
                    System.IO.File.Delete(Path.Combine(serverFolder, oldAvatarFileName));
                }

                if (FileUploadValidator.IsWebFriendlyImage(avatar))
                {
                    var fileName = FileStamp.MakeUnique(avatar.FileName);                    

                    avatar.SaveAs(Path.Combine(Server.MapPath(serverFolder), fileName));
                    user.AvatarPath = $"{serverFolder}/{fileName}";
                }
            }

            db.SaveChanges();

            foreach (var role in roleHelper.ListUserRoles(user.Id))
            {
                roleHelper.RemoveUserFromRole(user.Id, role);
            }

            if (roleName != "")
            {
                roleHelper.AddUserToRole(user.Id, roleName);
            }

            // remove user from all projects
            foreach (var p in projectHelper.ListUserProjects(user.Id))
            {
                projectHelper.RemoveUserFromProject(user.Id, p.Id);
            }

            // add user only to selected projects
            if (projectIds != null)
            {
                foreach (var Id in projectIds)
                {
                    projectHelper.AddUserToProject(user.Id, Id);
                }
            }
            
            return RedirectToAction("Index", "Home");
        }

    }
}