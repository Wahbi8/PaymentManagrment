using PaymentManagement.Domain;

namespace PaymentManagement.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> GetPaymentById(Guid id);
        Task<List<Payment>> GetAllPayments();
        Task<List<Payment>> GetPaymentByInvoiceId(Guid invoiceId);
        Task<List<Payment>> GetAllPaymentsByUserId(Guid userId);
        Task AddPayment(Payment payment);
        Task DeletePayment(Guid id);
        Task UpdatePayment(Payment payment);
    }
}
