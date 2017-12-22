using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker_Phoenix.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }

        #region Nav Section
        //Parent(s)
        public Ticket Ticket { get; set; }
        public ApplicationUser User { get; set; }
        #endregion
    }
}