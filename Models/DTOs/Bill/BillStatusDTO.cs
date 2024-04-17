using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Bill
{
    public class BillStatusDTO
    {
        public int BillStatusId { get; set; }
        public string BillStatusName { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
