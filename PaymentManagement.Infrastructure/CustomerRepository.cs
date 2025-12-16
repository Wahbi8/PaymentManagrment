using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentManagement.Domain.Entities;

namespace PaymentManagement.Infrastructure
{
    public class CustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext contex)
        {
            _context = contex;
        }

        public async Task<Customer> GetCustomerById(Guid id) => await _context.Customer.FindAsync(id);

        public async Task<List<Customer>> GetAllCustomers() => await _context.Customer.ToListAsync();

        public async Task<bool> AddCustomer(Customer customer)
        {
            await _context.Customer.AddAsync(customer);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCustomer(Guid id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null) return false;

            _context.Customer.Remove(customer);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCustemer(Customer customer)
        {
            var c = await _context.Customer.FindAsync(customer.Id);
            if (c == null) return false;

            c.CompanyId = customer.CompanyId;
            c.Name = customer.Name;
            c.Email = customer.Email;
            c.UpdatedAt = DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
