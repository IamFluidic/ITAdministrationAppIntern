using System;
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
        public string Title { get; set; }
        public string Description { get; set; }
        public string RequestedBy { get; set; }
        public DateTime? RequestedOn { get; set; }
        public int TicketStatusID { get; set; }

        public string AssignTo { get; set; }
        public string AssignedBy { get; set; }
        public DateTime? LastUpdated { get; set; }

        public string TicketStatusName
        {
            get
            {
                return StatusDictionary.ContainsKey(TicketStatusID) ? StatusDictionary[TicketStatusID] : "Unknown";
            }
        }

        // A dictionary to map status IDs to names
        private static readonly Dictionary<int, string> StatusDictionary = new Dictionary<int, string>
    {
        { 0, "Pending" },
        { 1, "Open" },
        { 2, "Pending" },
        { 3, "Closed" }
    };
    }
}
    

