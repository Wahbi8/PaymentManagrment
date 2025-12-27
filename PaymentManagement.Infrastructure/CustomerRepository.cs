using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentManagement.Domain;
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

        public async Task AddCustomer(Customer customer)
        {
            await _context.Customer.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomer(Guid id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null) throw new BusinessException("Can't find the customer");

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomer(Customer customer)
        {
            _context.Customer.Update(customer);
            await _context.SaveChangesAsync();
        }
    }
}
