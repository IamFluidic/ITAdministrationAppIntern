using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    public class TicketViewModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; } = null;
        public string? AssignTo { get; set; }
        public string? AssignedBy { get; set; }
    }
}
