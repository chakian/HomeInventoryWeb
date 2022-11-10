using HomeInv.Persistence.Dbo;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeInv.Persistence
{
    public class HomeInventoryDbContext : IdentityDbContext<User>
    {
        public HomeInventoryDbContext(DbContextOptions<HomeInventoryDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<AreaUser> AreaUsers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Home> Homes { get; set; }
        public virtual DbSet<HomeUser> HomeUsers { get; set; }
        public virtual DbSet<ItemDefinition> ItemDefinitions { get; set; }
        public virtual DbSet<ItemStockActionType> ItemStockActionTypes { get; set; }
        public virtual DbSet<ItemStockAction> ItemStockActions { get; set; }
        public virtual DbSet<ItemStock> ItemStocks { get; set; }
        public virtual DbSet<SizeUnit> SizeUnits { get; set; }
        public virtual DbSet<UserSetting> UserSettings { get; set; }
        public virtual DbSet<ItemUnitPrice> ItemUnitPrices { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            AuditableDboConfiguration.Configure(builder);

            builder.Entity<SizeUnit>()
                .Property(sizeUnit => sizeUnit.ConversionMultiplierToBase)
                .HasPrecision(28, 14);

            builder.Entity<ItemDefinition>()
                .HasOne(item => item.Category)
                .WithMany(category => category.ItemDefinitions)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ItemStock>()
                .HasOne(itemStock => itemStock.SizeUnit)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ItemStock>()
                .HasOne(itemStock => itemStock.ItemDefinition)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ItemStock>()
                .HasOne(itemStock => itemStock.Area)
                .WithMany(area => area.ItemStocks)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ItemStock>()
                .Property(itemStock => itemStock.RemainingAmount)
                .HasPrecision(18, 2);

            builder.Entity<ItemStockAction>()
                .HasOne(itemStockAction => itemStockAction.ItemStock)
                .WithMany(itemStock => itemStock.ItemStockActions)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ItemStockAction>()
                .HasOne(itemStockAction => itemStockAction.ItemStockActionType)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ItemStockAction>()
                .Property(itemStockAction => itemStockAction.Size)
                .HasPrecision(18, 2);

            builder.Entity<ItemStockAction>()
                .Property(itemStockAction => itemStockAction.Price)
                .HasColumnType("money");

            builder.Entity<ItemStockAction>()
                .Property(itemStockAction => itemStockAction.Currency)
                .HasColumnType("nvarchar(10)");

            builder.Entity<ItemUnitPrice>()
                .HasOne(itemUnitPrice => itemUnitPrice.ItemStockAction)
                .WithMany();

            builder.Entity<ItemUnitPrice>()
                .HasOne(itemUnitPrice => itemUnitPrice.ItemDefinition)
                .WithMany();

            builder.Entity<ItemUnitPrice>()
                .Property(b => b.UnitPrice)
                .HasColumnType("money");

            builder.Entity<AreaUser>()
                .HasOne(user => user.Area)
                .WithMany(area => area.AreaUsers)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<HomeUser>()
                .HasOne(user => user.User)
                .WithMany()
                .IsRequired()
                .HasForeignKey(user => user.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AreaUser>()
                .HasOne(user => user.User)
                .WithMany()
                .IsRequired()
                .HasForeignKey(user => user.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserSetting>()
                .HasOne(setting => setting.DefaultHome)
                .WithMany()
                .IsRequired()
                .HasForeignKey(setting => setting.DefaultHomeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserSetting>()
                .HasOne(setting => setting.User)
                .WithOne()
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
