namespace Models.DTOs.Bill
{
    public class BillDetailDTO
    {
        public int SubscriptionId { get; set; }
        public int ConsumptionBillId { get; set; }
        public double UnitsConsumed { get; set; }
        public int DaysBilled { get; set; }
        public double PricePerUnit { get; set; }
    }
}