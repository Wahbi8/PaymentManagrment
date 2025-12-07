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

        public async Task<bool> AddInvoice(Invoice invoice)
        {
            await _context.Invoice.AddAsync(invoice);
            int res = await _context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<bool> DeleteInvoice(Guid id)
        {
            var invoice = await _context.Invoice.FindAsync(id);
            if (invoice != null) return false;

            _context.Invoice.Remove(invoice);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateInvoice(Invoice invoice)
        {
            var i = await _context.Invoice.FindAsync(invoice.Id);
            if (i == null) return false;

            i.CompanyId = invoice.CompanyId;
            i.CreatedAt = invoice.CreatedAt;
            i.UpdatedAt = DateTime.UtcNow;
            i.IssueDate = invoice.IssueDate;
            i.DueDate = invoice.DueDate;
            i.TotalAmount = invoice.TotalAmount;
            i.AmountPaid = invoice.AmountPaid;
            i.IsPaid = invoice.IsPaid;

            return await _context.SaveChangesAsync() > 0;
        }


    }
}
