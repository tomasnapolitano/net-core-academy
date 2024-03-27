using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Service
{
    public class ConsumptionDTO
    {
        public int DaysOfConsumption { get; set; }
        public float UnitsConsumed {  get; set; }
        public float TotalCost { get; set; }
        public ServiceSubscriptionWithUserDTO ServiceSubscription { get; set; }
    }
}
