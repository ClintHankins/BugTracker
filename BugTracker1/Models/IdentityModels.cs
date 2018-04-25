using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTracker1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }

        public virtual ICollection<Projects> Projects { get; set; }
        public virtual ICollection<TicketAttachments> Attachments { get; set; }
        public virtual ICollection<TicketNotifications> Notifications { get; set; }
        public virtual ICollection<TicketComments> Comments { get; set; }
        public virtual ICollection<TicketHistory> History { get; set; }


        public ApplicationUser()
        {
            Projects = new HashSet<Projects>();
            Attachments = new HashSet<TicketAttachments>();
            Notifications = new HashSet<TicketNotifications>();
            Comments = new HashSet<TicketComments>();
            History = new HashSet<TicketHistory>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<TicketComments> Comments { get; set; }
        public DbSet<TicketAttachments> Attachments { get; set; }
        public DbSet<TicketHistory> Histories { get; set; }
        public DbSet<TicketNotifications> Notifications { get; set; }
        public DbSet<TicketPriority> Priorities { get; set; }
        public DbSet<TicketStatus> Statuses { get; set; }
        public DbSet<TicketType> Types { get; set; }
        public DbSet<Projects> Projects { get; set; }
    }
}