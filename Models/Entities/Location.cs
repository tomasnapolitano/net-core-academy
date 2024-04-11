namespace Models.Entities
{
    public partial class Location
    {
        public Location()
        {
            Addresses = new HashSet<Address>();
        }

        public int LocationId { get; set; }
        public string LocationName { get; set; } = null!;
        public int? DistrictId { get; set; }
        public string PostalCode { get; set; } = null!;

        public virtual District? District { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
