using PaymentManagement.Domain;
using PaymentManagement.Infrastructure;

namespace PaymentManagement.Application.Services
{
    public class InvoiceServices
    {
        private readonly InvoiceRepository _invoiceRepository;
        public InvoiceServices(InvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<Invoice> GetInvoiceById(Guid id)
        {
            if (id == Guid.Empty)
                throw new BusinessException("Invoice id cannot be empty");
            var invoice = await _invoiceRepository.GetInvoiceById(id);
            if (invoice == null)
                throw new BusinessException("Invoice not found");
            return invoice;
        }

        public async Task<List<Invoice>> GetInvoicesByCompanyId(Guid companyId)
        {
            if (companyId == Guid.Empty)
                throw new BusinessException("Company id cannot be empty");
            var invoices = await _invoiceRepository.GetInvoicesByCompanyId(companyId);
            if (invoices == null || !invoices.Any())
                throw new BusinessException("No invoices found for the company");
            return invoices;
        }

        public async Task AddInvoice(Invoice invoice)
        {
            if (invoice == null)
                throw new BusinessException($"{nameof(Invoice)} cannot be empty");
            invoice.Id = Guid.NewGuid();
            await _invoiceRepository.AddInvoice(invoice);
        }

        public async Task UpdateInvoice(Invoice invoice)
        {
            if (invoice == null)
                throw new BusinessException($"{nameof(Invoice)} cannot be empty");

            if (invoice.Status != InvoiceStatus.draft)
                throw new BusinessException("Only draft invoices can be updated.");

            await _invoiceRepository.UpdateInvoice(invoice);
        }

        public async Task DeleteInvoice(Guid id)
        {
            if (id == Guid.Empty)
                throw new BusinessException("Invoice id cannot be empty");

            var invoice = await _invoiceRepository.GetInvoiceById(id);
            if (invoice.Status != InvoiceStatus.draft)
                throw new BusinessException("Only draft invoices can be deleted.");

            await _invoiceRepository.DeleteInvoice(id);
        }

        public async Task SendInvoiceEmail(Guid invoiceId, string recipientEmail)
        {
            if (invoiceId == Guid.Empty)
                throw new BusinessException("Invoice id cannot be empty");
            if (string.IsNullOrWhiteSpace(recipientEmail))
                throw new BusinessException("Recipient email cannot be empty");
            var invoice = await _invoiceRepository.GetInvoiceById(invoiceId);
            if (invoice == null)
                throw new BusinessException("Invoice not found");

            // Logic to send email (omitted for brevity)

            //To  be added

            //if sent

            invoice.send();
        }

        public async Task CancelInvoice(Guid id)
        {
            if (id == Guid.Empty)
                throw new BusinessException("Invoice id cannot be empty");
            var invoice = await _invoiceRepository.GetInvoiceById(id);
            if (invoice == null)
                throw new BusinessException("Invoice not found");
            invoice.cancel();
            await _invoiceRepository.UpdateInvoice(invoice);
        }

        public async Task CheckInvoiceDueDate(Guid id)
        {
            if (id == Guid.Empty)
                throw new BusinessException("Invoice id cannot be empty");
            var invoice = await _invoiceRepository.GetInvoiceById(id);
            if (invoice == null)
                throw new BusinessException("Invoice not found");
            if (DateTime.UtcNow > invoice.DueDate && invoice.Status == InvoiceStatus.sent)
            {
                invoice.markAsOverdue();
            }
        }

    }
}
