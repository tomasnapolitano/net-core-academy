using System;
using System.Collections.Generic;

namespace Models.Entities
{
    public partial class ServiceType
    {
        public ServiceType()
        {
            Services = new HashSet<Service>();
        }

        public int ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<Service> Services { get; set; }
    }
}
