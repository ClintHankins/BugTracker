using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker1.Models
{
    public class ProjectDetailsView
    {
        public int Id { get; set; }
        public Projects Projects { get; set; }
        public ApplicationUser ProjectManager { get; set; }
    }
}