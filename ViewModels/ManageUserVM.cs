using AphidBT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AphidBT.ViewModels
{
    public class ManageUserVM
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AvatarPath { get; set; }
        public string Role { get; set; }
        //public IEnumerable<Project> AssignedProjects { get; set; }
        //public IEnumerable<SelectListItem> AllProjects { get; set; }
    }
}