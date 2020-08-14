using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AphidBT.Models
{
    public class Project
    {
        public int Id { get; set; } // this holds the primary key

        #region Actual Properties
        // Name is required
        // Data annotation to restrict the name length to no more that 50 characters
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public bool  IsArchived { get; set; }
        #endregion


        #region Parents/Children
        // an ICollection variable must be instantiated in the constructor
        // with a HastSet
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
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