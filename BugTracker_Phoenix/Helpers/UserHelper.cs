using BugTracker_Phoenix.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace BugTracker_Phoenix.Helpers
{
    public class UserHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static object Identity { get; internal set; }

        public static string GetUserName()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            if (userId != null)
            {
                var user = db.Users.Find(userId);
                return string.Format("{0} {1}", user.FirstName, user.LastName);
            }
            else
                return "Guest User";
          
        }

        public static string GetUserPhoto()
        {
            var photoPath = WebConfigurationManager.AppSettings["unisexphoto"];
            var userId = HttpContext.Current.User.Identity.GetUserId();
            if(!String.IsNullOrEmpty(userId))
            {
                var user = db.Users.Find(userId);
                if (!String.IsNullOrEmpty(user.MediaUrl))
                    photoPath = user.MediaUrl;
                else if(!String.IsNullOrEmpty(user.Gender))
                {
                    if(user.Gender == "Male")
                        photoPath = WebConfigurationManager.AppSettings["malephoto"];
                    else
                        photoPath = WebConfigurationManager.AppSettings["femalephoto"];
                }
            }
            return photoPath;
        }
    }
}