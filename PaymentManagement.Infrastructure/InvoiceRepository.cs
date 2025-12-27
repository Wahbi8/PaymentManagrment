using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentManagement.Domain;

namespace PaymentManagement.Infrastructure
{
    public class InvoiceRepository
    {
        private readonly AppDbContext _context;

        public InvoiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Invoice>> GetAllInvoices() => await _context.Invoice.ToListAsync();

        public async Task<Invoice> GetInvoiceById(Guid id) => await _context.Invoice.FindAsync(id);

        public async Task AddInvoice(Invoice invoice)
        {
            await _context.Invoice.AddAsync(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteInvoice(Guid id)
        {
            var invoice = await _context.Invoice.FindAsync(id);
            if (invoice != null) throw new BusinessException("Can't find the invoice");

            _context.Invoice.Remove(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateInvoice(Invoice invoice)
        {
            _context.Invoice.Update(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Invoice>> GetInvoicesByCompanyId(Guid companyId) =>
            await _context.Invoice.Where(i => i.CompanyId == companyId).ToListAsync();
    }
}
