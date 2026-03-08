using PaymentManagement.Domain;

namespace PaymentManagement.Domain.Interfaces
{
    public interface IInvoiceLineItemRepository
    {
        Task<InvoiceLineItem> GetById(Guid id);
        Task<List<InvoiceLineItem>> GetByInvoiceId(Guid invoiceId);
        Task Add(InvoiceLineItem lineItem);
        Task AddRange(List<InvoiceLineItem> lineItems);
        Task Update(InvoiceLineItem lineItem);
        Task Delete(Guid id);
    }
}
