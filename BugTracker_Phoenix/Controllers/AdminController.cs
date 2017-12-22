using BugTracker_Phoenix.Helpers;
using BugTracker_Phoenix.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker_Phoenix.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectsHelper projectHelper = new ProjectsHelper();
      
        // GET: Admin
        public ActionResult ProjectAssign(string id)
        {
            //Get a listing of project I am already assigned to
            var myProjectIds = projectHelper.ListUserProjects(id).Select(p => p.Id);
            ViewBag.UserId = id;
            ViewBag.AllProjects = new MultiSelectList(db.Projects, "Id", "Name", myProjectIds);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectAssign(string UserId, List<int> AllProjects)
        {
            //First use the Project helper to remove this user from all Project
            foreach(var project in projectHelper.ListUserProjects(UserId))
            {
                projectHelper.RemoveUserFromProject(UserId, project.Id);
            }

            //Next add the user to the selected Projects (AllProject)
            if(AllProjects != null)
            {
                foreach(var projectId in AllProjects)
                {
                    projectHelper.AddUserToProject(UserId, projectId);
                }
            }
            return RedirectToAction("UserIndex");
        }


        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult RoleAssign(string id)
        {
            ViewBag.UserId = id;
            var occupiedRole = roleHelper.ListUserRoles(id).FirstOrDefault();

            var mostRoles = db.Roles.Where(r => r.Name != "Admin");
            ViewBag.AllRoles = new SelectList(mostRoles, "Name", "Name", occupiedRole);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAssign(string UserId, string AllRoles)
        {
            //Add back any role that was selected
            if (String.IsNullOrEmpty(UserId))
            {
                return RedirectToAction("RoleAssign", new { id = UserId });
            }
            
            //Spin through all roles currently occupied removing the user from them all
            foreach(var role in roleHelper.ListUserRoles(UserId))
            {
                roleHelper.RemoveUserFromRole(UserId, role);
            }      
            
            if(!String.IsNullOrEmpty(AllRoles))
            {
                roleHelper.AddUserToRole(UserId, AllRoles);
                //Adding code to assign the user to the selected role
               
            }
            return RedirectToAction("UserIndex");
        }

        // GET: Admin
        [Authorize]
        public ActionResult UserIndex(string role = "")
        {
            var userList = db.Users.ToList();
            if (!String.IsNullOrEmpty(role))
                userList = roleHelper.UsersInRole(role).ToList();

            ViewBag.MyRole = roleHelper.ListUserRoles(User.Identity.GetUserId()).FirstOrDefault();
            return View(userList);
        }

        public ActionResult ClearRoles(string userId)
        {
            var roles = roleHelper.ListUserRoles(userId);
            foreach(var role in roles)
            {
                roleHelper.RemoveUserFromRole(userId, role);
            }
            return RedirectToAction("UserIndex");

        }

    }
}

