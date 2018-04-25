using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker1.Models
{
    public class Tickets
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public int ProjectsId { get; set; }
        public int TicketTypeId { get; set; }
        public int TicketPriorityId { get; set; }
        public int TicketStatusId { get; set; }
        public string OwnerUserId { get; set; }
        public string AssignedToUserId { get; set; }

        public virtual Projects Projects { get; set; }
        public virtual TicketType TicketType { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }
        public virtual ApplicationUser OwnerUser { get; set; }
        public virtual ApplicationUser AssignedToUser { get; set; }

        public virtual ICollection<TicketComments> Comments { get; set; }
        public virtual ICollection<TicketAttachments> Attachments { get; set; }
        public virtual ICollection<TicketHistory> History { get; set; }
        public virtual ICollection<TicketNotifications> Notifications { get; set; }

        public Tickets()
        {
            Comments = new HashSet<TicketComments>();
            Attachments = new HashSet<TicketAttachments>();
            History = new HashSet<TicketHistory>();
            Notifications = new HashSet<TicketNotifications>();
        }
    }
}