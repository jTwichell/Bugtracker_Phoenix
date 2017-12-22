using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker_Phoenix.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime ChangeDate { get; set; }
        public string UserId { get; set; }

        #region Nav Section
        //Parent(s)
        public Ticket Ticket { get; set; }
        public ApplicationUser User { get; set; }

        #endregion

    }
}