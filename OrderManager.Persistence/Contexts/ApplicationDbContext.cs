using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderManager.Domain.Entities;
using OrderManager.Domain.States;
using OrderManager.Persistence.Identity;

namespace OrderManager.Persistence.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<OrderStateAudit> OrderStateAudits { get; set; } 
        public DbSet<RevokedToken> RevokedTokens { get; set; } 
        public DbSet<DeliveryStaff> DeliveryStaffs { get; set; } 

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.HasDefaultSchema("security");

            modelBuilder.Entity<ApplicationUser>(entity => entity.ToTable(name: "AspNetUsers", "security"));
            modelBuilder.Entity<IdentityRole>(entity => entity.ToTable(name: "AspNetRoles", "security"));
            modelBuilder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("AspNetUserRoles", "security"));
            modelBuilder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable("AspNetUserClaims", "security"));
            modelBuilder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("AspNetUserLogins", "security"));
            modelBuilder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable("AspNetRoleClaims", "security"));
            modelBuilder.Entity<IdentityUserToken<string>>(entity => entity.ToTable("AspNetUserTokens", "security"));

            modelBuilder.Entity<RevokedToken>(entity =>
            {
                entity.ToTable("RevokedTokens", "security");

                entity.HasKey(rt => rt.Id);

                entity.Property(rt => rt.Token)
                    .IsRequired()
                    .HasMaxLength(500); 

                entity.Property(rt => rt.RevokedAt)
                    .IsRequired();

                entity.Property(rt => rt.Expiration)
                    .IsRequired();
            });
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasColumnType("ltree");
            });
            // Ignore non-entity types
            modelBuilder.Ignore<OrderStateBase>();
            modelBuilder.Ignore<PendingState>();
            modelBuilder.Ignore<PreparingState>();
            modelBuilder.Ignore<ReadyForDeliveryState>();

            // DeliveryStaff Entity Configuration
            modelBuilder.Entity<DeliveryStaff>(entity =>
            {
                entity.ToTable("DeliveryStaffs", "public");

                entity.HasKey(ds => ds.Id);
                entity.HasOne<ApplicationUser>()
                      .WithOne() 
                      .HasForeignKey<DeliveryStaff>(ds => ds.Id)
                      .OnDelete(DeleteBehavior.Cascade); 
                entity.HasOne(ds => ds.CurrentOrder)
                      .WithOne() 
                      .HasForeignKey<DeliveryStaff>(ds => ds.CurrentOrderId)
                      .OnDelete(DeleteBehavior.SetNull); 

                // Configure properties
                entity.Property(ds => ds.IsAvailable)
                      .IsRequired()
                      .HasDefaultValue(true);

                entity.Property(ds => ds.CurrentLocation)
                      .HasMaxLength(255);
                entity.Ignore(ds => ds.User);
            });


            modelBuilder.Entity<OrderStateAudit>(entity =>
            {
                entity.ToTable("OrderStateAudits", "public");
                entity.HasKey(audit => audit.Id);

                entity.Property(audit => audit.FromState)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(audit => audit.ToState)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(audit => audit.StaffId)
                    .IsRequired();

                entity.Property(audit => audit.Timestamp)
                    .IsRequired();
            });

            // Configure Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders", "public");
                entity.HasKey(o => o.Id);

                entity.Property(o => o.DeliveryOption)
                    .HasConversion<string>()
                    .IsRequired();

                entity.Property(o => o.State)
                    .HasConversion<string>()
                    .IsRequired();

                entity.Property(o => o.SpecialInstructions)
                    .HasMaxLength(500)
                    .IsRequired(false);

                entity.Property(o => o.OrderDate)
                    .IsRequired();

                entity.Property(o => o.CompletedDate)
                    .IsRequired(false);
                entity.Property(o => o.OrderLocation)
                .IsRequired(false);

                entity.HasMany(o => o.Items)
                    .WithOne()
                    .HasForeignKey("OrderId") 
                    .OnDelete(DeleteBehavior.Cascade); 
            });

            // Configure OrderItem entity
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItems", "public");
                entity.HasKey(oi => oi.Id);

                entity.HasOne(oi => oi.MenuItem)
                    .WithMany()
                    .HasForeignKey(oi => oi.MenuItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(oi => oi.Price)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(oi => oi.Name)
                .HasMaxLength(100)
                .IsRequired(false);
                entity.Property(oi => oi.Quantity)
                    .IsRequired();
            });
   
            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.ToTable("MenuItems", "public");
                entity.HasKey(m => m.Id);

                entity.Property(m => m.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(m => m.Description)
                    .HasMaxLength(500);

                entity.Property(m => m.Price)
                    .HasColumnType("decimal(18,2)");

                entity.Property(m => m.IsAvailable)
                    .HasDefaultValue(true);

                entity.Property(m => m.IsDeleted)
                    .HasDefaultValue(false);
            });
        }

    }
}
