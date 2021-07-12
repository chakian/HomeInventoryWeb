﻿using HomeInv.Persistence.Dbo;
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
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Home> Homes { get; set; }
        public virtual DbSet<HomeUser> HomeUsers { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemStock> ItemStocks { get; set; }
        public virtual DbSet<SizeUnit> SizeUnits { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            AuditableDboConfiguration.Configure(builder);

            //builder.Entity<Category>()
            //    .HasOne(category => category.ParentCategory)
            //    .WithMany(parent => parent.ChildCategories)
            //    .IsRequired(false);

            //builder.Entity<Item>()
            //    .HasOne(item => item.Category)
            //    .WithMany(category => category.Items)
            //    .IsRequired();

            //builder.Entity<ItemStock>()
            //    .HasOne(item => item.Area)
            //    .WithMany(area => area.ItemStocks)
            //    .IsRequired();

            //builder.Entity<ItemStock>()
            //    .HasOne(item => item.Container)
            //    .WithMany(container => container.ContainingItems)
            //    .IsRequired(false);

            builder.Entity<SizeUnit>()
                .Property(sizeUnit => sizeUnit.ConversionMultiplierToBase)
                .HasPrecision(28, 14);

            builder.Entity<Item>()
                .Property(item => item.Size)
                .HasPrecision(18, 2);

            //builder.Entity<Transaction>()
            //    .Property(b => b.Amount)
            //    .HasColumnType("money");

            //builder.Entity<Transaction>()
            //    .HasOne(t => t.Budget)
            //    .WithMany(b => b.Transactions)
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder.Entity<Account>()
            //    .Property(b => b.Balance)
            //    .HasColumnType("money");
        }
    }
}
