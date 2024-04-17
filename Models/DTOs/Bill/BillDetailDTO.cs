using Models.DTOs.Service;

namespace Models.DTOs.Bill
{
    public class BillDetailDTO
    {
        //public int SubscriptionId { get; set; }
        public ServiceSubscriptionDTO Subscription { get; set; }
        public int ConsumptionBillId { get; set; }
        public double UnitsConsumed { get; set; }
        public int DaysBilled { get; set; }
        public double PricePerUnit { get; set; }
    }
}