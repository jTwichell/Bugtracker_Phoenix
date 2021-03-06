﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker_Phoenix.Models
{
    public class TicketNotification
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime Created { get; set; }
        public int TicketId { get; set; }
        public string SenderId { get; set; }
        public string RecipientId { get; set; }

        #region Nav Section
        //Parent(s)
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Recipient { get; set; }
        #endregion
    }
}