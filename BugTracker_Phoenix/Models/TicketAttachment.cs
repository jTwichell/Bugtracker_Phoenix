using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker_Phoenix.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string FilePath { get; set; }
        public string Descripion { get; set; }
        public DateTime Created { get; set; }
        public string UserId { get; set; }


        #region Nav Section
        //Parent(s)
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }

        #endregion
    }
}