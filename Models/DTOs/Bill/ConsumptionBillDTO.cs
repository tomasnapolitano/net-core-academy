using Models.DTOs.User;

namespace Models.DTOs.Bill
{
    public class ConsumptionBillDTO
    {
        //public int UserId { get; set; }
        public int ConsumptionBillId { get; set; }
        public UserDTO User { get; set; }
        public BillStatusDTO BillStatus { get; set; }
        public DateTime BillDate { get; set; }
        public double Total { get; set; }
        public List<BillDetailDTO> BillDetails { get; set; }
    }
}
