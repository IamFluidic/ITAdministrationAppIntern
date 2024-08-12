using TicketManagement.Models;

namespace ITAdministrationApp.Areas.Admin.Models
{
    public class LogTicketModel
    {
        public int RowNumber { get; set; }
        public int LogID { get; set; }
        public int TicketID { get; set; }
        public string Title { get; set; }

        public int PrevTicketStatusID { get; set; }
        public int CurrentTicketStatusID { get; set; }

        public DateTime AddedOn { get; set; }
        public string AddedBy { get; set; }
        public string Remarks { get; set; }

       
        public string TicketTitle   { get; set; }   //Title form ServiceTicket
        public string PreTicketStatus
        {
            get
            {
                return StatusDictionary.ContainsKey(PrevTicketStatusID) ? StatusDictionary[PrevTicketStatusID] : "Unknown";
            }
        }


        public string CurrTicketStatus
        {
            get
            {
                return StatusDictionary.ContainsKey(CurrentTicketStatusID) ? StatusDictionary[CurrentTicketStatusID] : "Unknown";
            }
        }



        // A dictionary to map status IDs to names
        private static readonly Dictionary<int, string>
            StatusDictionary = new Dictionary<int, string>
            {
            { 1, "Open" },
            { 2, "Pending" },
            { 3, "Closed" }
            };
    }

}

