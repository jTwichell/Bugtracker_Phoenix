using BugTracker_Phoenix.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using BugTracker_Phoenix.Helpers;

namespace BugTracker_Phoenix.Helpers
{
    public class TicketHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projHelper = new ProjectsHelper();

        public List<Ticket> GetProjectTickets(int projectId)
        {
            var project = db.Projects.Find(projectId);
            return project.Tickets.ToList();
        }

        public List<Ticket> GetPMTickets()
        {
            var pmTickets = new List<Ticket>();
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var pmProjects = projHelper.ListUserProjects(userId);

            foreach(var project in pmProjects)
            {
                foreach(var ticket in project.Tickets)
                {
                    pmTickets.Add(ticket);
                }
            }
            return pmTickets;
        }

        public void AddTicketHistory(Ticket oldTicket, Ticket newTicket)
        {
            //Write a for loop that loops through the properties of a Ticket
            foreach (var property in Utilities.BuildPropertyHistoryList())
            {
                //Having an issue with null property values...AssignToUserId
                var newValue = newTicket.GetType().GetProperty(property) == null ? "" : newTicket.GetType().GetProperty(property).GetValue(newTicket).ToString();
                var oldValue = oldTicket.GetType().GetProperty(property) == null ? "" : oldTicket.GetType().GetProperty(property).GetValue(oldTicket).ToString();

                if(newValue != oldValue)
                {
                    //Add TicketHistory...
                    var newTicketHistory = new TicketHistory();
                    newTicketHistory.UserId = HttpContext.Current.User.Identity.GetUserId();
                    newTicketHistory.ChangeDate = DateTime.Now;
                    newTicketHistory.TicketId = newTicket.Id;
            
                    //Record Property name and values
                    newTicketHistory.Property = property;
                    newTicketHistory.OldValue = oldValue;
                    newTicketHistory.NewValue = newValue;
                 
                    db.TicketHistories.Add(newTicketHistory);
                    db.SaveChanges();
                }
            }
        }

        public void AddTicketNotification(int ticketId, string recipientId, string message)
        {
            var notification = new TicketNotification
            {
                TicketId = ticketId,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                RecipientId = recipientId,
                Body = message,
                Created = DateTime.Now
            };
            db.TicketNotifications.Add(notification);
            db.SaveChanges();
        }

        public async Task GenerateNotifications(Ticket oldTicket, Ticket newTicket)
        {
            var ticketState = "";
            if(oldTicket.AssignToUserId == null)
            {
                if (newTicket.AssignToUserId == null)
                    ticketState = "NotAssigned";
                else
                    ticketState = "Assigned";
            }
            else
            {
                if (newTicket.AssignToUserId == null)
                    ticketState = "UnAssigned";
                else if (oldTicket.AssignToUserId != newTicket.AssignToUserId)
                    ticketState = "ReAssigned";
            }

            switch (ticketState)
            {
                case "Assigned":
                    AddTicketNotification(newTicket.Id, newTicket.AssignToUserId, Utilities.BuildNotificationMessage("Assigned", newTicket.Id, newTicket.AssignToUserId));
                    await Utilities.SendEmailNotification(newTicket.AssignToUserId, Utilities.BuildNotificationMessage("Assigned", newTicket.Id, newTicket.AssignToUserId));
                    break;                    
                case "UnAssigned":
                    AddTicketNotification(newTicket.Id, oldTicket.AssignToUserId, Utilities.BuildNotificationMessage("UnAssigned", oldTicket.Id, oldTicket.AssignToUserId));
                    await Utilities.SendEmailNotification(oldTicket.AssignToUserId, Utilities.BuildNotificationMessage("Assigned", oldTicket.Id, oldTicket.AssignToUserId));
                    break;
                case "ReAssigned":
                    AddTicketNotification(newTicket.Id, oldTicket.AssignToUserId, Utilities.BuildNotificationMessage("UnAssigned", oldTicket.Id, oldTicket.AssignToUserId));
                    await Utilities.SendEmailNotification(oldTicket.AssignToUserId, Utilities.BuildNotificationMessage("Assigned", oldTicket.Id, oldTicket.AssignToUserId));

                    AddTicketNotification(newTicket.Id, newTicket.AssignToUserId, Utilities.BuildNotificationMessage("Assigned", newTicket.Id, newTicket.AssignToUserId));
                    await Utilities.SendEmailNotification(newTicket.AssignToUserId, Utilities.BuildNotificationMessage("Assigned", newTicket.Id, newTicket.AssignToUserId));
                    break;
                case "NotAssigned":
                    break;
            }
        }




    }
}