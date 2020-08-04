using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AphidBT.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; } // this holds the primary key

        #region Parents/Children
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        #endregion

        #region Actual Properties
        public string FilePath { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        #endregion
    }
}