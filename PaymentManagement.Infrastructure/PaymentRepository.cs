using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentManagement.Domain;

namespace PaymentManagement.Infrastructure
{
    public class PaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> GetPaymentById(Guid id) => await _context.Payment.FindAsync(id);

        public async Task<List<Payment>> GetAllPayments() => await _context.Payment.ToListAsync();

        public async Task<bool> AddPayment(Payment payment)
        {
            await _context.Payment.AddAsync(payment);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePayment(Guid id)
        {
            var payment = await _context.Payment.FindAsync();
            if (payment == null) return false;

            _context.Payment.Remove(payment);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePayment(Payment payment)
        {
            var p = await _context.Payment.FindAsync(payment.Id);
            if (p == null) return false;

            p.InvoiceId = payment.InvoiceId;
            p.Amount = payment.Amount;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
