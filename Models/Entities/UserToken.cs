using System;
using System.Collections.Generic;

namespace Models.Entities
{
    public partial class UserToken
    {
        public int TokenId { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = null!;
        public DateTime StartDate { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
