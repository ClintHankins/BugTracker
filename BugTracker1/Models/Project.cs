using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker1.Models
{
    public class Projects
    {
        public int Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public string Name { get; set; }
        public string ProjectMangerId { get; set; }
        public int ProjectsId { get; set; }

        public virtual ICollection<Tickets> Ticket { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }

        public Projects()
        {
            Users = new HashSet<ApplicationUser>();
            Ticket = new HashSet<Tickets>();
        }
    }
}