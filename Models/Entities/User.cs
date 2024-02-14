namespace Models.Entities
{
    public partial class User
    {
        public User()
        {
            ConsumptionBills = new HashSet<ConsumptionBill>();
            Districts = new HashSet<District>();
            ServiceSubscriptions = new HashSet<ServiceSubscription>();
            UserTokens = new HashSet<UserToken>();
        }

        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int AddressId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Dninumber { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public bool? IsConfirmed { get; set; }
        public bool? Active { get; set; }

        public virtual Address Address { get; set; } = null!;
        public virtual UserRole Role { get; set; } = null!;
        public virtual ICollection<ConsumptionBill> ConsumptionBills { get; set; }
        public virtual ICollection<District> Districts { get; set; }
        public virtual ICollection<ServiceSubscription> ServiceSubscriptions { get; set; }
        public virtual ICollection<UserToken> UserTokens { get; set; }
    }
}
