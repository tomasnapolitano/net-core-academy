using System;
using System.Collections.Generic;

namespace Models.Entities
{
    public partial class BillStatus
    {
        public BillStatus()
        {
            ConsumptionBills = new HashSet<ConsumptionBill>();
        }

        public int BillStatusId { get; set; }
        public string BillStatusName { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<ConsumptionBill> ConsumptionBills { get; set; }
    }
}
