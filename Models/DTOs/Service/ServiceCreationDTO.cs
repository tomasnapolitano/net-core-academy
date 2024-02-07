using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Service
{
    public class ServiceCreationDTO
    {
        public int ServiceTypeId { get; set; }
        public string ServiceName { get; set; } = null!;
        public double PricePerUnit { get; set; }
    }
}
