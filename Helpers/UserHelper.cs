using AphidBT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AphidBT.Helpers
{
    public class UserHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public string GetFullName(string userId)
        {
            var user = db.Users.Find(userId);
            var firstName = user.FirstName;
            var lastName = user.LastName;
            return firstName + " " + lastName;
        }

        public string LastNameFirst(string userId)
        {
            var user = db.Users.Find(userId);
            return user.FullName;
        }

        public List<ApplicationUser> GetUserList()
        {
            return db.Users.ToList();
        }
    }
}