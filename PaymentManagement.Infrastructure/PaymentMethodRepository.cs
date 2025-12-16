using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentManagement.Domain.Entities;

namespace PaymentManagement.Infrastructure
{
    public class PaymentMethodRepository
    {
        private readonly AppDbContext _context;

        public PaymentMethodRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentMethod> GetPaymentMethodById(Guid id) => await _context.PaymentMethod.FindAsync(id);

        public async Task<List<PaymentMethod>> GetAllPaymentMethods() => await _context.PaymentMethod.ToListAsync();

        public async Task<bool> AddPaymentMethod(PaymentMethod paymentMethod)
        {
            await _context.PaymentMethod.AddAsync(paymentMethod);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePaymentMethod(Guid id)
        {
            var paymentMethod = await _context.PaymentMethod.FindAsync(id);
            if (paymentMethod != null) return false;

            _context.PaymentMethod.Remove(paymentMethod);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePaymentMethod(PaymentMethod paymentMethod)
        {
            var PM = await _context.PaymentMethod.FindAsync(paymentMethod.Id);
            if (PM == null) return false;

            PM.CustomerId = paymentMethod.CustomerId;
            PM.Type = paymentMethod.Type;
            PM.PaymentToken = paymentMethod.PaymentToken;
            PM.CardBrand = paymentMethod.CardBrand;
            PM.Bank_name = paymentMethod.Bank_name;
            PM.UpdatedAt = DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
