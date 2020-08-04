using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AphidBT.Models
{
    public class Ticket
    {
        public int Id { get; set; } // this holds the primary key

        #region Parents/Children
        public int Projectid { get; set; }  // this is the foreign key to the Project table
        public int TicketPriorityId { get; set; }
        public int TicketStatusId { get; set; }
        public int TicketTypeId { get; set; }

        public string SubmitterId { get; set; }
        public string DeveloperId { get; set; }

        // the following gives the foreign keys a way to understand the navigation between
        // the parent/child relationship
        public virtual Project Project { get; set; }
        public virtual TicketType TicketType { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }

        public virtual ApplicationUser Submitter { get; set; }
        public virtual ApplicationUser Developer { get; set; }


        // Ticket is the parent of the following children
        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }
        public virtual ICollection<TicketComment> TicketComments { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual ICollection<TicketNotification> TicketNotifications { get; set; }
        #endregion

        #region Actual Properties

        // should limit the length of Title and Description
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }

        // the question mark in Updated means it's Null-able, which really means
        // it is not necessary
        public DateTime? Updated { get; set; }
        public bool IsResolved { get; set; }
        public bool IsArchived { get; set; }
        #endregion

        #region Constructor
        public Ticket()
        {
            TicketAttachments = new HashSet<TicketAttachment>();
            TicketComments = new HashSet<TicketComment>();
            TicketHistories = new HashSet<TicketHistory>();
            TicketNotifications = new HashSet<TicketNotification>();
        }
        #endregion
    }
}