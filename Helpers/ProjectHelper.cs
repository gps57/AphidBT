﻿using AphidBT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Ajax.Utilities;

namespace AphidBT.Helpers
{
    public class ProjectHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        public int UserProjectCount()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var count = ListUserProjects(userId).Count;

            if (roleHelper.IsUserInRole(userId, "Admin"))
            {
                count = db.Projects.Count();
            }
            else
            {
                count = ListUserProjects(userId).Count;
            }

            return count;
        }

        // Add one or more users to a project
        public void AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                Project project = db.Projects.Find(projectId);
                var user = db.Users.Find(userId);

                project.Users.Add(user);
                db.SaveChanges();
                //return;  // don't need this because the method returns a void
            }
        }

        // Remove one or more users from a project
        public bool RemoveUserFromProject(string userId, int projectId)
        {
            Project project = db.Projects.Find(projectId);
            var user = db.Users.Find(userId);

            bool result = project.Users.Remove(user);
            db.SaveChanges();

            return result;
        }

        // List users on a project
        public ICollection<ApplicationUser> ListUsersOnProject(int projectId)
        {
            //Project project = db.Projects.Find(projectId);
            //var resultList = new List<ApplicationUser>();
            //resultList.AddRange(project.Users);
            //return resultList;

            return db.Projects.Find(projectId).Users;

        }

        // List users not on a project
        public List<ApplicationUser> ListUsersNotOnProject(int projectId)
        {
            var resultList = new List<ApplicationUser>();

            foreach (var user in db.Users.ToList())
            {
                if (!IsUserOnProject(user.Id, projectId))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }

        // Boolean method IsUserOnProject
        public bool IsUserOnProject(string userId, int projectId)
        {

            Project project = db.Projects.Find(projectId);
            var user = db.Users.Find(userId);

            return project.Users.Contains(user);
        }


        // Filters the list of users on a project by role - use to populate ticket assignment dropdown
        public List<ApplicationUser> ListUsersOnProjectInRole(int projectId, string roleName)
        {
            var userList = ListUsersOnProject(projectId);
            var resultList = new List<ApplicationUser>();
            var roleHelper = new UserRolesHelper();

            foreach (var user in userList)
            {
                if (roleHelper.IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }

        public ICollection<Project> ListUserProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var projects = user.Projects.ToList();
            return projects;
        }

        // OPTIONAL: List users not on a project in a specific role
        // OPTIONAL: Filter for Project Manager role - if you code for only one allowed
    }
}