using BugTracker_Phoenix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker_Phoenix.ViewModel
{
    public class DashboardVM
    {
        public List<Project> Projects { get; set; }
        public List<Ticket> Tickets { get; set; }

        public DashboardVM()
        {
            Projects = new List<Project>();
            Tickets = new List<Ticket>();
        }
    }
}