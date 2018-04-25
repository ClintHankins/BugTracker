namespace BugTracker1.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker1.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var store = new RoleStore<IdentityRole>(context);
            var manager = new RoleManager<IdentityRole>(store);
            var role = new IdentityRole();

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                role = new IdentityRole { Name = "Admin" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                role = new IdentityRole { Name = "Developer" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                role = new IdentityRole { Name = "Submitter" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                role = new IdentityRole { Name = "ProjectManager" };
                manager.Create(role);
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (!context.Users.Any(u => u.Email == "clinthankins@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "clinthankins@gmail.com",
                    Email = "clinthankins@gmail.com",
                    FirstName = "Clint",
                    LastName = "Hankins",
                    FullName = "Clint Hankins"
                };

                userManager.Create(user, "Abc123!");

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "Admin"
                    });
            }
            if (!context.Users.Any(u => u.Email == "manager@email.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "manager@email.com",
                    Email = "manager@email.com",
                    FirstName = "Manager",
                    LastName = "Role",
                    FullName = "MANGR"
                };

                userManager.Create(user, "Abc123!");

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "ProjectManager"
                    });
            }
            if (!context.Users.Any(u => u.Email == "developer@email.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "developer@email.com",
                    Email = "developer@email.com",
                    FirstName = "Developer",
                    LastName = "Role",
                    FullName = "DEVPR"
                };

                userManager.Create(user, "Abc123!");

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "Developer"
                    });
            }
            if (!context.Users.Any(u => u.Email == "submitter@email.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "submitter@email.com",
                    Email = "submitter@email.com",
                    FirstName = "Submitter",
                    LastName = "Role",
                    FullName = "SUBMT"
                };

                userManager.Create(user, "Abc123!");

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "Submitter"
                    });
            }


            if (!context.Priorities.Any(u => u.Name == "High"))
            { context.Priorities.Add(new TicketPriority { Name = "High" }); }

            if (!context.Priorities.Any(u => u.Name == "Medium"))
            { context.Priorities.Add(new TicketPriority { Name = "Medium" }); }

            if (!context.Priorities.Any(u => u.Name == "Low"))
            { context.Priorities.Add(new TicketPriority { Name = "Low" }); }

            if (!context.Priorities.Any(u => u.Name == "Urgent"))
            { context.Priorities.Add(new TicketPriority { Name = "Urgent" }); }

            if (!context.Types.Any(u => u.Name == "Production Fix"))
            { context.Types.Add(new TicketType { Name = "Production Fix" }); }

            if (!context.Types.Any(u => u.Name == "Project Task"))
            { context.Types.Add(new TicketType { Name = "Project Task" }); }

            if (!context.Types.Any(u => u.Name == "Software Update"))
            { context.Types.Add(new TicketType { Name = "Software Update" }); }

            if (!context.Statuses.Any(u => u.Name == "New"))
            { context.Statuses.Add(new TicketStatus { Name = "New" }); }

            if (!context.Statuses.Any(u => u.Name == "In Development"))
            { context.Statuses.Add(new TicketStatus { Name = "In Development" }); }

            if (!context.Statuses.Any(u => u.Name == "Completed"))
            { context.Statuses.Add(new TicketStatus { Name = "Completed" }); }


        }
    }
}