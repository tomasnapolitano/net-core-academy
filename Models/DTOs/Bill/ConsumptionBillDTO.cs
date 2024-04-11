namespace Models.DTOs.Bill
{
    public class ConsumptionBillDTO
    {
        public int UserId { get; set; }
        public int BillStatusId { get; set; }
        public DateTime BillDate { get; set; }
        public double Total { get; set; }
        public List<BillDetailDTO> BillDetails { get; set; }
    }
}
