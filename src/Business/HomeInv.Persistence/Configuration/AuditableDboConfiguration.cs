using HomeInv.Persistence.Dbo;
using Microsoft.EntityFrameworkCore;

namespace HomeInv.Persistence
{
    public class AuditableDboConfiguration
    {
        /// <summary>
        /// This method should register ForeignKeys related to AspNetUsers table when User class needs a virtual IEnumerable reference to the foreign key table.
        /// Which is probably never.
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void Configure(ModelBuilder modelBuilder)
        {
            Configure<Area>(modelBuilder);
            Configure<AreaUser>(modelBuilder);
            Configure<Home>(modelBuilder);
            Configure<HomeUser>(modelBuilder);
            //Configure<Item>(modelBuilder);
            //Configure<ItemStock>(modelBuilder);
            Configure<UserSetting>(modelBuilder);
        }

        private static void Configure<T>(ModelBuilder modelBuilder, bool isUserBindingRequired = false)
            where T : BaseAuditableDbo
        {
            if (isUserBindingRequired)
            {
                modelBuilder.Entity<T>()
                    .HasOne(s => s.InsertUser)
                    .WithMany()
                    .HasForeignKey(e => e.InsertUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<T>()
                    .HasOne(s => s.UpdateUser)
                    .WithMany()
                    .HasForeignKey(e => e.UpdateUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            }

            modelBuilder.Entity<T>()
                .Property(t => t.InsertTime)
                .HasDefaultValueSql("getdate()");
        }
    }
}
