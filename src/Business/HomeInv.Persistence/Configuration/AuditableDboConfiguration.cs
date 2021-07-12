using HomeInv.Persistence.Dbo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //Configure<HomeUser>(modelBuilder);
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
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<T>()
                .HasOne(s => s.UpdateUser)
                .WithMany()
                .HasForeignKey(e => e.UpdateUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
