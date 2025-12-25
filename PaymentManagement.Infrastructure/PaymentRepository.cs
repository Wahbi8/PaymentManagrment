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

        public async Task<List<Payment>> GetPaymentByInvoiceId(Guid invoiceId) =>
            await _context.Payment.Where(p => p.InvoiceId == invoiceId).ToListAsync();

        public async Task AddPayment(Payment payment)
        {
            await _context.Payment.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePayment(Guid id)
        {
            var payment = await _context.Payment.FindAsync();
            if (payment == null) throw new BusinessException("Can't find the payment");

            _context.Payment.Remove(payment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePayment(Payment payment)
        {
            _context.Payment.Update(payment);
            await _context.SaveChangesAsync();
        }
    }
}
