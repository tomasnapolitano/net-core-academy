namespace Models.Entities
{
    public partial class ConsumptionBill
    {
        public ConsumptionBill()
        {
            BillDetails = new HashSet<BillDetail>();
        }

        public int ConsumptionBillId { get; set; }
        public int UserId { get; set; }
        public int BillStatusId { get; set; }
        public DateTime BillDate { get; set; }
        public double Total { get; set; }

        public virtual BillStatus BillStatus { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<BillDetail> BillDetails { get; set; }
    }
}
