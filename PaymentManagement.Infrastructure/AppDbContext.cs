using Microsoft.EntityFrameworkCore;
using PaymentManagement.Domain;
using PaymentManagement.Domain.Entities;
using PaymentManagement.Domain.models;

namespace PaymentManagement.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
    }
}
