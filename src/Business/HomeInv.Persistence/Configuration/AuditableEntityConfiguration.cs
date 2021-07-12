using HomeInv.Persistence.Dbo;
using Microsoft.EntityFrameworkCore;

namespace HomeInv.Persistence
{
    public class AuditableEntityConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            Configure<Home>(modelBuilder);
            Configure<HomeUser>(modelBuilder);
            //Configure<Item>(modelBuilder);
            //Configure<ItemStock>(modelBuilder);
            //Configure<Area>(modelBuilder);
        }

        private static void Configure<T>(ModelBuilder modelBuilder)
            where T : BaseAuditableDbo
        {
            modelBuilder.Entity<T>()
                .HasOne(s => s.InsertUser)
                .WithMany()
                .HasForeignKey(e => e.InsertUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<T>()
                .HasOne(s => s.UpdateUser)
                .WithMany()
                .HasForeignKey(e => e.UpdateUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
