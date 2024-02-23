using Models.DTOs.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.District
{
    public class DistrictWithServicesDTO : DistrictDTO
    {
        public IList<ServiceDTO> Services { get; set; }
    }
}