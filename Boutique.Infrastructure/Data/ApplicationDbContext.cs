using Boutique.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Reflection.Emit;

namespace Boutique.Infrastructure.Data {
    public class ApplicationDbContext:IdentityDbContext<User,IdentityRole<Guid>,Guid> {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public DbSet<Address> Addresses { get; set; }
        protected override void OnModelCreating(ModelBuilder builder) {

            base.OnModelCreating(builder);

            builder.Entity<Payment>()
     .HasKey(p => p.Id);

            builder.Entity<Payment>()
                .HasMany(p => p.Transactions)
                .WithOne(pt => pt.Payment)
                .HasForeignKey(pt => pt.PaymentId);


            builder.Entity<Address>()
       .HasKey(a => a.Id);
        }
    }
}
