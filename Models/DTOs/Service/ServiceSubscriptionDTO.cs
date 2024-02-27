using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Service
{
    public class ServiceSubscriptionDTO
    {
        public int SubscriptionId { get; set; }
        public int? UserId { get; set; }
        public int? DistrictXserviceId { get; set; }
        public DateTime StartDate { get; set; }
        public bool PauseSubscription { get; set; }
        public ServiceDTO Service { get; set; }
    }
}
