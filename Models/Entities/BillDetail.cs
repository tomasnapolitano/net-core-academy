using System;
using System.Collections.Generic;

namespace Models.Entities
{
    public partial class BillDetail
    {
        public int BillDetailId { get; set; }
        public int SubscriptionId { get; set; }
        public int ConsumptionBillId { get; set; }
        public double UnitsConsumed { get; set; }

        public virtual ConsumptionBill ConsumptionBill { get; set; } = null!;
        public virtual ServiceSubscription Subscription { get; set; } = null!;
    }
}
