namespace Models.Entities
{
    public partial class DistrictXservice
    {
        public DistrictXservice()
        {
            ServiceSubscriptions = new HashSet<ServiceSubscription>();
        }

        public int DistrictXserviceId { get; set; }
        public int? DistrictId { get; set; }
        public int? ServiceId { get; set; }
        public bool? Status { get; set; }

        public virtual District? District { get; set; }
        public virtual Service? Service { get; set; }
        public virtual ICollection<ServiceSubscription> ServiceSubscriptions { get; set; }
    }
}
