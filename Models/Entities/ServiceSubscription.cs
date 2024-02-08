namespace Models.Entities
{
    public partial class ServiceSubscription
    {
        public ServiceSubscription()
        {
            BillDetails = new HashSet<BillDetail>();
        }

        public int SubscriptionId { get; set; }
        public int? UserId { get; set; }
        public int? DistrictXserviceId { get; set; }
        public DateTime StartDate { get; set; }
        public bool PauseSubscription { get; set; }

        public virtual DistrictXservice? DistrictXservice { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<BillDetail> BillDetails { get; set; }
    }
}
