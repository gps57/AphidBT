﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AphidBT.Models
{
    public class TicketNotification
    {
        public int Id { get; set; } // holds the primary key

        #region Parents/Children
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        #endregion

        #region Actual Properties
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
        public bool IsRead { get; set; }
        #endregion
    }
}