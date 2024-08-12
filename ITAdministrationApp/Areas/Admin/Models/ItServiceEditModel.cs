namespace ITAdministrationApp.Areas.Admin.Models
{
    public class ItServiceEditModel
    {
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public string BuyFrom { get; set; }
        public DateTime? BuyDate { get; set; } // Nullable for flexibility
        public DateTime? ExpiredOn { get; set; } // Nullable for services without expiry
        public DateTime? LastPaidDate { get; set; } // Nullable for services without recurring payments
        public decimal? PaidAmount { get; set; }
        public string UsedInDomains { get; set; }
        public string ServiceType { get; set; }
        public bool IsActive { get; set; } = true; // Set default value to active
        public bool IsDeleted { get; set; } = false; // Set default value to not deleted
        
        //public bool ItStatus { get; set; } = false;
        // No AddedBy, AddedOn, DeletedBy, DeletedOn, UpdatedBy, UpdatedOn
    }
}
