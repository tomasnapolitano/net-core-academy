using Models.DTOs.District;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.User
{
    public class AddressDTO
    {
        public int AddressId { get; set; }
        public string StreetName { get; set; } = null!;
        public string Neighborhood { get; set; } = null!;
        public string StreetNumber { get; set; } = null!;

        public LocationDTO? Location { get; set; }
    }
}
