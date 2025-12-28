using PaymentManagement.Domain;
using PaymentManagement.Infrastructure;

namespace PaymentManagement.Application.Services
{
    public class PaymentMethodServices
    {
        private readonly PaymentMethodRepository _paymentMethodRepo;

        public PaymentMethodServices(PaymentMethodRepository paymentMethodRepo)
        {
            _paymentMethodRepo = paymentMethodRepo;
        }

        public async Task<List<PaymentMethod>> GetPaymentMethodsByUserId(Guid id)
        {
            if (id == Guid.Empty)
                throw new BusinessException("UserId cannot be empty");
            var paymentMethods = await _paymentMethodRepo.GetPaymentMethodsByUserId(id);
            if (paymentMethods == null || !paymentMethods.Any())
                throw new BusinessException("No payment methods found for the user");
            return paymentMethods;
        }

        public async Task AddPaymentMethod(PaymentMethod paymentMethod)
        {
            if (paymentMethod == null)
                throw new BusinessException($"{nameof(PaymentMethod)} must be filled");
            paymentMethod.Id = Guid.NewGuid();
            await _paymentMethodRepo.AddPaymentMethod(paymentMethod);
        }

        public async Task<PaymentMethod> GetPaymentMethodById(Guid id)
        {
            if (id == Guid.Empty)
                throw new BusinessException("PM id cannot be empty");
            var pm = await _paymentMethodRepo.GetPaymentMethodById(id);
            if (pm == null)
                throw new BusinessException("Payment method not found");
            return pm;
        }

        public async Task UpdatePaymentMethod(PaymentMethod paymentMethod)
        {
            if (paymentMethod == null)
                throw new BusinessException($"{nameof(PaymentMethod)} cannot be empty");

            await _paymentMethodRepo.UpdatePaymentMethod(paymentMethod);
        }

        public async Task DeletePaymentMethod(Guid id)
        {
            if (id == Guid.Empty)
                throw new BusinessException("PM id cannot be empty");

            await _paymentMethodRepo.DeletePaymentMethod(id);
        }
    }
}
