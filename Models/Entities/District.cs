namespace Models.Entities
{
    public partial class District
    {
        public District()
        {
            DistrictXservices = new HashSet<DistrictXservice>();
            Locations = new HashSet<Location>();
        }

        public int DistrictId { get; set; }
        public string DistrictName { get; set; } = null!;
        public int? AgentId { get; set; }

        public virtual User? Agent { get; set; }
        public virtual ICollection<DistrictXservice> DistrictXservices { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
    }
}
