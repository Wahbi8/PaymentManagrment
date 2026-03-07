using PaymentManagement.Domain;

namespace PaymentManagement.Domain.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<List<Invoice>> GetAllInvoices();
        Task<Invoice> GetInvoiceById(Guid id);
        Task AddInvoice(Invoice invoice);
        Task DeleteInvoice(Guid id);
        Task UpdateInvoice(Invoice invoice);
        Task<List<Invoice>> GetInvoicesByCompanyId(Guid companyId);
    }
}
