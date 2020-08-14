using AphidBT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AphidBT.ViewModels
{
    public class TicketsListVM
    {
        public List<Ticket> AllTickets { get; set; }
        public List<Ticket> MyTickets { get; set; }
    }
}