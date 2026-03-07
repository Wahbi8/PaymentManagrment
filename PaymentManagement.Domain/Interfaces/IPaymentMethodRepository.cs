using PaymentManagement.Domain;

namespace PaymentManagement.Domain.Interfaces
{
    public interface IPaymentMethodRepository
    {
        Task<PaymentMethod> GetPaymentMethodById(Guid id);
        Task<List<PaymentMethod>> GetAllPaymentMethods();
        Task<List<PaymentMethod>> GetPaymentMethodsByUserId(Guid id);
        Task AddPaymentMethod(PaymentMethod paymentMethod);
        Task DeletePaymentMethod(Guid id);
        Task UpdatePaymentMethod(PaymentMethod paymentMethod);
    }
}
