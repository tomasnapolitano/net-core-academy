using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.District
{
    public class LocationDTO
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; } = null!;
        public string PostalCode { get; set; } = null!;

        public virtual DistrictDTO District { get; set; }
    }
}
