﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker1.Models
{
    public class TicketComments
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTimeOffset Created { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }
        //nav properties
        public virtual Tickets Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}