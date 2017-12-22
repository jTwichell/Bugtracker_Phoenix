namespace BugTracker_Phoenix.Migrations
{
    using BugTracker_Phoenix.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker_Phoenix.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker_Phoenix.Models.ApplicationDbContext context)
        {
            #region Role Creation Region
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }
            #endregion

            #region User Creation Region
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            //Check for the existence of an email in the Users table
            if (!context.Users.Any(u => u.Email == "Admin@mailinator.com"))
            {
                //If none is found then create a new user with that email
                userManager.Create(new ApplicationUser
                {
                    UserName = "Admin@mailinator.com",
                    Email = "Admin@mailinator.com",
                    FirstName = "Jason",
                    LastName = "Twichell",
                    DisplayName = "Admin",
                    MediaUrl = ""
                }, "Abc&123!"); // <-- password for this user
            }

            if (!context.Users.Any(u => u.Email == "ProjectManager1@mailinator.com"))
            {
                //If none is found then create a new user with that email
                userManager.Create(new ApplicationUser
                {
                    UserName = "ProjectManager1@mailinator.com",
                    Email = "ProjectManager1@mailinator.com",
                    FirstName = "Project",
                    LastName = "Manager 1",
                    DisplayName = "PM1",
                    MediaUrl = ""
                }, "Abc&123!"); // <-- password for this user
            }

            if (!context.Users.Any(u => u.Email == "ProjectManager2@mailinator.com"))
            {
                //If none is found then create a new user with that email
                userManager.Create(new ApplicationUser
                {
                    UserName = "ProjectManager2@mailinator.com",
                    Email = "ProjectManager2@mailinator.com",
                    FirstName = "Project",
                    LastName = "Manager 2",
                    DisplayName = "PM2",
                    MediaUrl = ""
                }, "Abc&123!"); // <-- password for this user
            }

            if (!context.Users.Any(u => u.Email == "Developer1@mailinator.com"))
            {
                //If none is found then create a new user with that email
                userManager.Create(new ApplicationUser
                {
                    UserName = "Developer1@mailinator.com",
                    Email = "Developer1@mailinator.com",
                    FirstName = "Developer1",
                    LastName = "Developer",
                    DisplayName = "Dev1",
                    MediaUrl = ""
                }, "Abc&123!"); // <-- password for this user
            }

            if (!context.Users.Any(u => u.Email == "Developer2@mailinator.com"))
            {
                //If none is found then create a new user with that email
                userManager.Create(new ApplicationUser
                {
                    UserName = "Developer2@mailinator.com",
                    Email = "Developer2@mailinator.com",
                    FirstName = "Developer2 ",
                    LastName = "Developer",
                    DisplayName = "Dev2",
                    MediaUrl = ""
                }, "Abc&123!"); // <-- password for this user
            }

            if (!context.Users.Any(u => u.Email == "Submitter1@mailinator.com"))
            {
                //If none is found then create a new user with that email
                userManager.Create(new ApplicationUser
                {
                    UserName = "Submitter1@mailinator.com",
                    Email = "Submitter1@mailinator.com",
                    FirstName = "Sub",
                    LastName = "Mitter1",
                    DisplayName = "The Submitter",
                    MediaUrl = ""
                }, "Abc&123!"); // <-- password for this user
            }

            if (!context.Users.Any(u => u.Email == "Submitter2@mailinator.com"))
            {
                //If none is found then create a new user with that email
                userManager.Create(new ApplicationUser
                {
                    UserName = "Submitter2@mailinator.com",
                    Email = "Submitter2@mailinator.com",
                    FirstName = "Sub",
                    LastName = "Mitter2",
                    DisplayName = "The Submitter",
                    MediaUrl = ""
                }, "Abc&123!"); // <-- password for this user
            }
            #endregion

            #region Role Assignment Region
            var userId = userManager.FindByEmail("Admin@mailinator.com").Id;
            userManager.AddToRole(userId, "Admin");

            userId = userManager.FindByEmail("ProjectManager1@mailinator.com").Id;
            userManager.AddToRole(userId, "ProjectManager");

            userId = userManager.FindByEmail("ProjectManager2@mailinator.com").Id;
            userManager.AddToRole(userId, "ProjectManager");

            userId = userManager.FindByEmail("Developer1@mailinator.com").Id;
            userManager.AddToRole(userId, "Developer");

            userId = userManager.FindByEmail("Developer2@mailinator.com").Id;
            userManager.AddToRole(userId, "Developer");

            userId = userManager.FindByEmail("Submitter1@mailinator.com").Id;
            userManager.AddToRole(userId, "Submitter");

            userId = userManager.FindByEmail("Submitter2@mailinator.com").Id;
            userManager.AddToRole(userId, "Submitter");

            #endregion

            #region Seed TicketType table

            context.TicketTypes.AddOrUpdate(
                t => t.Name,
                    new TicketType { Id = 100, Name = "Bug" },
                    new TicketType { Id = 200, Name = "Task" },
                    new TicketType { Id = 300, Name = "Documentation" });
            #endregion

            #region Seed Ticket Status
            context.TicketStatus.AddOrUpdate(
                t => t.Name,
                    new TicketStatus { Id = 100, Name = "Open/Unassigned" },
                    new TicketStatus { Id = 200, Name = "Open/Assigned" },
                    new TicketStatus { Id = 300, Name = "In Progress" },
                    new TicketStatus { Id = 400, Name = "Closed" },
                    new TicketStatus { Id = 500, Name = "Archived" },
                    new TicketStatus { Id = 600, Name = "Waiting for Documentation" }
            );
            #endregion

            #region Seed Ticket Priority
            context.TicketPriorities.AddOrUpdate(
                t => t.Name,
                    new TicketPriority { Id = 100, Name = "Immediate" },
                    new TicketPriority { Id = 200, Name = "Highest" },
                    new TicketPriority { Id = 300, Name = "High" },
                    new TicketPriority { Id = 400, Name = "Medium" },
                    new TicketPriority { Id = 500, Name = "Low" },
                    new TicketPriority { Id = 600, Name = "Lowest" },
                    new TicketPriority { Id = 700, Name = "None" }
            );
            #endregion

            #region Seed Projects
            context.Projects.AddOrUpdate(
                b => b.Name,
                   new Project
                   {
                       Id = 10010,
                       Name = "Test Project 1",
                       Description = "This is a project description"
                   },
                   new Project
                   {
                       Id = 10020,
                       Name = "Test Project 2",
                       Description = "This is a project description"
                   },
                   new Project
                   {
                       Id = 10030,
                       Name = "Test Project 3",
                       Description = "This is a project description"
                   },
                   new Project
                   {
                       Id = 10040,
                       Name = "Test Project 4",
                       Description = "This is a project description"
                   }
            );
            #endregion

            #region Seed Tickets
            context.Tickets.AddOrUpdate(
                 t => t.Title,
                    new Ticket
                    {
                        Title = "Ticket 1, Project 1",
                        Created = DateTime.Now,
                        Description = "This is a ticket description",
                        OwnerUserId = userManager.FindByEmail("Submitter1@mailinator.com").Id,
                        ProjectId = 10010,
                        TicketPriorityId = 200,
                        TicketStatusId = 100,
                        TicketTypeId = 100                        
                    },
                    new Ticket
                    {
                        Title = "Ticket 2, Project 1",
                        Created = DateTime.Now,
                        Description = "This is a ticket description",
                        OwnerUserId = userManager.FindByEmail("Submitter2@mailinator.com").Id,
                        ProjectId = 10010,
                        TicketPriorityId = 200,
                        TicketStatusId = 100,
                        TicketTypeId = 100
                    },
                    new Ticket
                    {
                        Title = "Ticket 1, Project 2",
                        Created = DateTime.Now,
                        Description = "This is a ticket description",
                        OwnerUserId = userManager.FindByEmail("Submitter1@mailinator.com").Id,
                        ProjectId = 10020,
                        TicketPriorityId = 200,
                        TicketStatusId = 100,
                        TicketTypeId = 100
                    },
                    new Ticket
                    {
                        Title = "Ticket 2, Project 2",
                        Created = DateTime.Now,
                        Description = "This is a ticket description",
                        OwnerUserId = userManager.FindByEmail("Submitter2@mailinator.com").Id,
                        ProjectId = 10020,
                        TicketPriorityId = 200,
                        TicketStatusId = 100,
                        TicketTypeId = 100
                    },
                    new Ticket
                    {
                        Title = "Ticket 1, Project 3",
                        Created = DateTime.Now,
                        Description = "This is a ticket description",
                        OwnerUserId = userManager.FindByEmail("Submitter1@mailinator.com").Id,
                        ProjectId = 10030,
                        TicketPriorityId = 200,
                        TicketStatusId = 100,
                        TicketTypeId = 100
                    },
                    new Ticket
                    {
                        Title = "Ticket 2, Project 3",
                        Created = DateTime.Now,
                        Description = "This is a ticket description",
                        OwnerUserId = userManager.FindByEmail("Submitter2@mailinator.com").Id,
                        ProjectId = 10030,
                        TicketPriorityId = 200,
                        TicketStatusId = 100,
                        TicketTypeId = 100
                    },
                    new Ticket
                    {
                        Title = "Ticket 1, Project 4",
                        Created = DateTime.Now,
                        Description = "This is a ticket description",
                        OwnerUserId = userManager.FindByEmail("Submitter1@mailinator.com").Id,
                        ProjectId = 10040,
                        TicketPriorityId = 200,
                        TicketStatusId = 100,
                        TicketTypeId = 100
                    },
                    new Ticket
                    {
                        Title = "Ticket 2, Project 4",
                        Created = DateTime.Now,
                        Description = "This is a ticket description",
                        OwnerUserId = userManager.FindByEmail("Submitter2@mailinator.com").Id,
                        ProjectId = 10040,
                        TicketPriorityId = 200,
                        TicketStatusId = 100,
                        TicketTypeId = 100
                    }
             );
            #endregion
        }
    }
}
