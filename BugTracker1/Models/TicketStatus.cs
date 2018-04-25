using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker1.Models
{
    public class TicketStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Tickets> Ticket { get; set; }
    }
}