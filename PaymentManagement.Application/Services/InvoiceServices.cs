using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            await _invoiceRepository.UpdateInvoice(invoice);
        }

        public async Task DeleteInvoice(Guid id)
        {
            if (id == Guid.Empty)
                throw new BusinessException("Invoice id cannot be empty");
            await _invoiceRepository.DeleteInvoice(id);
        }

    }
}
