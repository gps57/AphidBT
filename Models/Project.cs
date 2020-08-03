using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AphidBT.Models
{
    public class Project
    {
        public int Id { get; set; }

        #region Parents/Children
        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        #endregion

        #region Actual Properties
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public bool  IsArchived { get; set; }
        #endregion

        #region Constructor
        public Project()
        {
            // if you have an ICollection property, you need to have a HashSet in your constructor
            Tickets = new HashSet<Ticket>();
            Users = new HashSet<ApplicationUser>();
        }
        #endregion
    }
}