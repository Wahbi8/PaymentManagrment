using Microsoft.EntityFrameworkCore;
using PaymentManagement.Domain;

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

        public async Task<List<PaymentMethod>> GetPaymentMethodsByUserId(Guid id) =>
            await _context.PaymentMethod.Where(p => p.CustomerId == id).ToListAsync();
        public async Task AddPaymentMethod(PaymentMethod paymentMethod)
        {
            await _context.PaymentMethod.AddAsync(paymentMethod);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePaymentMethod(Guid id)
        {
            var paymentMethod = await _context.PaymentMethod.FindAsync(id);
            if (paymentMethod == null) throw new BusinessException("Can't find the payment method");

            _context.PaymentMethod.Remove(paymentMethod);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePaymentMethod(PaymentMethod paymentMethod)
        {
            var pm = await _context.PaymentMethod.FindAsync(paymentMethod.Id);
            if (pm == null)
                throw new BusinessException($"can't find {nameof(PaymentMethod)} in DB");

            _context.PaymentMethod.Update(paymentMethod);
            await _context.SaveChangesAsync();
        }
    }
}
