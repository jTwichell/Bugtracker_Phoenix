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
                    dashboardData.Projects = db.Projects.ToList();
                    dashboardData.Tickets = db.Tickets.ToList();
                    break;
                case "ProjectManager":
                    dashboardData.Projects = projHelper.ListUserProjects(userId).ToList();
                    dashboardData.Tickets = tixHelper.GetPMTickets();
                    break;
                case "Developer":
                    dashboardData.Projects = projHelper.ListUserProjects(userId).ToList();
                    dashboardData.Tickets = db.Tickets.Where(t => t.AssignToUserId == userId).ToList();
                    break;
                case "Submitter":                   
                    dashboardData.Tickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
                    break;
                default:
                    ViewBag.Message = "You will not be able to see any data until you are assigned to a role.";
                    break;
            }
            return View(dashboardData);
        }

        #region Dashboard Action for each Role...
        //// GET: Dashboard
        //[Authorize(Roles = "Admin")]
        //public ActionResult AdminDashboard()
        //{
        //    var adminDB = new DashboardVM();
        //    adminDB.Projects = db.Projects.ToList();
        //    adminDB.Tickets = db.Tickets.ToList();
        //    return View("Dashboard",adminDB);
        //}

        //// GET: Dashboard
        //[Authorize(Roles = "ProjectManager")]
        //public ActionResult PMDashboard()
        //{
        //    var pmDB = new DashboardVM();
        //    pmDB.Projects = projHelper.ListUserProjects(User.Identity.GetUserId()).ToList();
        //    pmDB.Tickets = tixHelper.GetPMTickets();
        //    return View("Dashboard", pmDB);
        //}

        //// GET: Dashboard
        //[Authorize(Roles = "Submitter")]
        //public ActionResult SubmitterDashboard()
        //{
        //    var userId = User.Identity.GetUserId();
        //    var subDB = new DashboardVM();
        //    subDB.Tickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
        //    return View("Dashboard", subDB);
        //}

        //// GET: Dashboard
        //[Authorize(Roles = "Developer")]
        //public ActionResult DeveloperDashboard()
        //{
        //    var devDB = new DashboardVM();
        //    devDB.Projects = db.Projects.ToList();
        //    devDB.Tickets = db.Tickets.ToList();
        //    return View("Dashboard", devDB);
        //}
        #endregion
    }
}