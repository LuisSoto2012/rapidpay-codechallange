using System;
using Microsoft.EntityFrameworkCore;
using RapidPay.Data.Extensions;
using RapidPay.Domain;

namespace RapidPay.Data
{
	public class RapidPayContext : DbContext
	{
        public DbSet<User> Users { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<PaymentHistory> PaymentHistories { get; set; }
        public DbSet<PaymentFee> PaymentFees { get; set; }

        public RapidPayContext(DbContextOptions<RapidPayContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //User
            modelBuilder.Entity<User>().HasKey(k => k.Id);
            modelBuilder.Entity<User>().Property(k => k.Id).ValueGeneratedOnAdd().UseIdentityColumn();
            modelBuilder.Entity<User>().Property(k => k.Username).IsRequired();
            modelBuilder.Entity<User>().Property(k => k.Password).IsRequired();

            //Card
            modelBuilder.Entity<Card>().HasKey(k => k.Id);
            modelBuilder.Entity<Card>().Property(k => k.Id).ValueGeneratedOnAdd().UseIdentityColumn();
            modelBuilder.Entity<Card>().Property(k => k.Number).HasMaxLength(15);
            modelBuilder.Entity<Card>().Property(k => k.Number).IsRequired();
            modelBuilder.Entity<Card>().Property(k => k.Balance).IsRequired();
            modelBuilder.Entity<Card>()
               .HasIndex(u => u.Number)
               .IsUnique();
            modelBuilder.Entity<Card>()
            .HasMany(c => c.PaymentHistories)
            .WithOne(e => e.Card);

            //PaymentHistory
            modelBuilder.Entity<PaymentHistory>().HasKey(k => k.Id);
            modelBuilder.Entity<PaymentHistory>().Property(k => k.Id).ValueGeneratedOnAdd().UseIdentityColumn();
            modelBuilder.Entity<PaymentHistory>().Property(k => k.Payment).IsRequired();
            modelBuilder.Entity<PaymentHistory>().Property(k => k.Fee).IsRequired();

            //PaymentFee
            modelBuilder.Entity<PaymentFee>().HasKey(k => k.Id);
            modelBuilder.Entity<PaymentFee>().Property(k => k.Id).ValueGeneratedOnAdd().UseIdentityColumn();
            modelBuilder.Entity<PaymentFee>().Property(k => k.Fee).IsRequired();
            modelBuilder.Entity<PaymentFee>().Property(k => k.FeeDate).IsRequired();

        }
    }
}

