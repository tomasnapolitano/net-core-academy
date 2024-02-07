using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Service
{
    public class ServiceTypeDTO
    {
        public int ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
