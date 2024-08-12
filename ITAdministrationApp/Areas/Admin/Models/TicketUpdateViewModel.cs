using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Models
{
    public class TicketUpdateViewModel
    {
        public string Title {get; set;}
        public int TicketID { get; set; }
        [Display (Name ="Status")]
        public int TicketStatusID { get; set; }
        public string  Remarks { get; set; }
       
    }
}