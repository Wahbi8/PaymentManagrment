using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentManagement.Domain;
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
    }
}
