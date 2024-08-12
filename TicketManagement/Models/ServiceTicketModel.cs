﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    public class ServiceTicketModel
    {
        public int RowNumber { get; set; }
        public int TicketID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? RequestedBy { get; set; }
        public DateTime? RequestedOn { get; set; }
        //public TicketStatus TicketStatusID { get; set; }
        public int TicketStatusID { get; set; }
        public string? AssignTo { get; set; }
        public string? AssignedBy { get; set; }
    }
}

