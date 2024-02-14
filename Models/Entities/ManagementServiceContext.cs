using Microsoft.EntityFrameworkCore;

namespace Models.Entities
{
    public partial class ManagementServiceContext : DbContext
    {
        public ManagementServiceContext()
        {
        }

        public ManagementServiceContext(DbContextOptions<ManagementServiceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<BillDetail> BillDetails { get; set; } = null!;
        public virtual DbSet<BillStatus> BillStatuses { get; set; } = null!;
        public virtual DbSet<ConsumptionBill> ConsumptionBills { get; set; } = null!;
        public virtual DbSet<District> Districts { get; set; } = null!;
        public virtual DbSet<DistrictXservice> DistrictXservices { get; set; } = null!;
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public virtual DbSet<Log> Logs { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<ServiceSubscription> ServiceSubscriptions { get; set; } = null!;
        public virtual DbSet<ServiceType> ServiceTypes { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;
        public virtual DbSet<UserToken> UserTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.Neighborhood).HasMaxLength(255);

                entity.Property(e => e.StreetName).HasMaxLength(255);

                entity.Property(e => e.StreetNumber).HasMaxLength(50);

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__Address__Locatio__4E88ABD4");
            });

            modelBuilder.Entity<BillDetail>(entity =>
            {
                entity.ToTable("BillDetail");

                entity.HasOne(d => d.ConsumptionBill)
                    .WithMany(p => p.BillDetails)
                    .HasForeignKey(d => d.ConsumptionBillId)
                    .HasConstraintName("FK__BillDetai__Consu__5070F446");

                entity.HasOne(d => d.Subscription)
                    .WithMany(p => p.BillDetails)
                    .HasForeignKey(d => d.SubscriptionId)
                    .HasConstraintName("FK__BillDetai__Subsc__4F7CD00D");
            });

            modelBuilder.Entity<BillStatus>(entity =>
            {
                entity.ToTable("BillStatus");

                entity.Property(e => e.BillStatusName).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(100);
            });

            modelBuilder.Entity<ConsumptionBill>(entity =>
            {
                entity.ToTable("ConsumptionBill");

                entity.Property(e => e.BillDate).HasColumnType("datetime");

                entity.HasOne(d => d.BillStatus)
                    .WithMany(p => p.ConsumptionBills)
                    .HasForeignKey(d => d.BillStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Consumpti__BillS__52593CB8");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ConsumptionBills)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Consumpti__UserI__5165187F");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.ToTable("District");

                entity.Property(e => e.DistrictName).HasMaxLength(255);

                entity.HasOne(d => d.Agent)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.AgentId)
                    .HasConstraintName("FK__District__AgentI__5FB337D6");
            });

            modelBuilder.Entity<DistrictXservice>(entity =>
            {
                entity.ToTable("DistrictXService");

                entity.Property(e => e.DistrictXserviceId).HasColumnName("DistrictXServiceId");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.DistrictXservices)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK__DistrictX__Distr__534D60F1");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.DistrictXservices)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK__DistrictX__Servi__5441852A");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.Property(e => e.LocationName).HasMaxLength(255);

                entity.Property(e => e.PostalCode).HasMaxLength(50);

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK__Location__Distri__5535A963");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Level).HasMaxLength(10);

                entity.Property(e => e.Logger).HasMaxLength(255);

                entity.Property(e => e.Url).HasMaxLength(255);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.ServiceName).HasMaxLength(255);

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Service__Service__5629CD9C");
            });

            modelBuilder.Entity<ServiceSubscription>(entity =>
            {
                entity.HasKey(e => e.SubscriptionId)
                    .HasName("PK__ServiceS__9A2B249DA0043B23");

                entity.ToTable("ServiceSubscription");

                entity.Property(e => e.DistrictXserviceId).HasColumnName("DistrictXServiceId");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.DistrictXservice)
                    .WithMany(p => p.ServiceSubscriptions)
                    .HasForeignKey(d => d.DistrictXserviceId)
                    .HasConstraintName("FK__ServiceSu__Distr__571DF1D5");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ServiceSubscriptions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__ServiceSu__UserI__5812160E");
            });

            modelBuilder.Entity<ServiceType>(entity =>
            {
                entity.ToTable("ServiceType");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.ServiceTypeName).HasMaxLength(255);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.Dninumber)
                    .HasMaxLength(50)
                    .HasColumnName("DNINumber");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User__AdressId__59FA5E80");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User__RoleId__59063A47");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__UserRole__8AFACE1ABBBEE6E8");

                entity.ToTable("UserRole");

                entity.Property(e => e.RoleDescription).HasMaxLength(255);

                entity.Property(e => e.RoleName).HasMaxLength(255);
            });

            modelBuilder.Entity<UserToken>(entity =>
            {
                entity.HasKey(e => e.TokenId)
                    .HasName("PK__UserToke__658FEEEA453E9BC1");

                entity.ToTable("UserToken");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Token).HasMaxLength(255);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserTokens)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserToken__UserI__5AEE82B9");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
