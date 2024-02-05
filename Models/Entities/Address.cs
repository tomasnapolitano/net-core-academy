using System;
using System.Collections.Generic;

namespace Models.Entities
{
    public partial class Address
    {
        public Address()
        {
            Users = new HashSet<User>();
        }

        public int AddressId { get; set; }
        public string StreetName { get; set; } = null!;
        public string Neighborhood { get; set; } = null!;
        public string StreetNumber { get; set; } = null!;
        public int? LocationId { get; set; }

        public virtual Location? Location { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
