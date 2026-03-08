using PaymentManagement.Domain;

namespace PaymentManagement.Domain.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<List<Invoice>> GetAllInvoices(int pageNumber, int pageSize);
        Task<int> GetInvoicesCount();
        Task<Invoice> GetInvoiceById(Guid id);
        Task AddInvoice(Invoice invoice);
        Task DeleteInvoice(Guid id);
        Task UpdateInvoice(Invoice invoice);
        Task<List<Invoice>> GetInvoicesByUserId(Guid userId, int pageNumber, int pageSize);
        Task<int> GetInvoicesCountByUserId(Guid userId);
    }
}
