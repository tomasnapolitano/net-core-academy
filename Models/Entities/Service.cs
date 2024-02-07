namespace Models.Entities
{
    public partial class Service
    {
        public Service()
        {
            DistrictXservices = new HashSet<DistrictXservice>();
        }

        public int ServiceId { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceName { get; set; } = null!;
        public double PricePerUnit { get; set; }

        public virtual ServiceType ServiceType { get; set; } = null!;
        public virtual ICollection<DistrictXservice> DistrictXservices { get; set; }
    }
}
