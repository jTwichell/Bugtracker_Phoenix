using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker_Phoenix.Models
{
    public class TicketType
    {
        public int Id { get; set; }

        [Display(Name = "Type")]
        public string Name { get; set; }

        #region Nav Section
        public virtual ICollection<Ticket> Tickets { get; set; }

        public TicketType()
        {
            Tickets = new HashSet<Ticket>();
        }
        #endregion
    }
}