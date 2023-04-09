using Microsoft.EntityFrameworkCore;
using PaymentSimple.Core.Domain.Models;

namespace PaymentSimple.DataAccess
{
    public class DataContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DataContext()
        {

        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.Contact).WithMany(p => p.Cards).HasForeignKey(d => d.ContactId);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.Card).WithMany(p => p.Orders).HasForeignKey(d => d.CardId);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.Card).WithMany(p => p.Payments).HasForeignKey(d => d.CardId);
                entity.HasOne(d => d.Order).WithMany(p => p.Payments).HasForeignKey(d => d.OrderId);
            });
        }

    }
}
