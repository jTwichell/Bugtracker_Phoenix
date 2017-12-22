using BugTracker_Phoenix.Helpers;
using BugTracker_Phoenix.Models;
using BugTracker_Phoenix.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker_Phoenix.Controllers
{
    public class DashboardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projHelper = new ProjectsHelper();
        private TicketHelper tixHelper = new TicketHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        public ActionResult BasicDashboard()
        {
            var dashboardData = new DashboardVM();
            var userId = User.Identity.GetUserId();
            var myRole = "Guest";
            if (userId != null)
                myRole = roleHelper.ListUserRoles(User.Identity.GetUserId()).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                    dashboardData.RecentProjects = db.Projects.ToList();
                    dashboardData.RecentTickets = db.Tickets.OrderByDescending(tn => tn.Created).Take(5).ToList();
                    dashboardData.RecentNotifications = db.TicketNotifications.OrderByDescending(tn => tn.Created).Take(5).ToList();
                    dashboardData.RecentHistories = db.TicketHistories.OrderByDescending(th => th.ChangeDate).Take(5).ToList();
                    
                    break;
                case "ProjectManager":
                    dashboardData.RecentProjects = projHelper.ListUserProjects(userId).ToList();
                    dashboardData.RecentTickets = tixHelper.GetPMTickets();
                    break;
                case "Developer":
                    dashboardData.RecentProjects = projHelper.ListUserProjects(userId).ToList();
                    dashboardData.RecentTickets = db.Tickets.Where(t => t.AssignToUserId == userId).ToList();
                    break;
                case "Submitter":                   
                    dashboardData.RecentTickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
                    break;
                default:
                    ViewBag.Message = "You will not be able to see any data until you are assigned to a role.";
                    break;
            }
            return View(dashboardData);
        }

      
    }
}