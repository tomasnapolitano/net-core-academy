using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Service
{
    public class DistrictXserviceDTO
    {
        public int DistrictXserviceId { get; set; }
        public int? DistrictId { get; set; }
        public int? ServiceId { get; set; }
        public bool Active { get; set; }
    }
}
