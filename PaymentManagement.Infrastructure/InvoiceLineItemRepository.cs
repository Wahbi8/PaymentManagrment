using Microsoft.EntityFrameworkCore;
using PaymentManagement.Domain;
using PaymentManagement.Domain.Interfaces;

namespace PaymentManagement.Infrastructure
{
    public class InvoiceLineItemRepository : IInvoiceLineItemRepository
    {
        private readonly AppDbContext _context;

        public InvoiceLineItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<InvoiceLineItem> GetById(Guid id)
            => await _context.InvoiceLineItem.FindAsync(id) 
               ?? throw new BusinessException("Line item not found");

        public async Task<List<InvoiceLineItem>> GetByInvoiceId(Guid invoiceId)
            => await _context.InvoiceLineItem.Where(x => x.InvoiceId == invoiceId).ToListAsync();

        public async Task Add(InvoiceLineItem lineItem)
        {
            await _context.InvoiceLineItem.AddAsync(lineItem);
            await _context.SaveChangesAsync();
        }

        public async Task AddRange(List<InvoiceLineItem> lineItems)
        {
            await _context.InvoiceLineItem.AddRangeAsync(lineItems);
            await _context.SaveChangesAsync();
        }

        public async Task Update(InvoiceLineItem lineItem)
        {
            _context.InvoiceLineItem.Update(lineItem);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var lineItem = await _context.InvoiceLineItem.FindAsync(id);
            if (lineItem == null) throw new BusinessException("Line item not found");
            
            _context.InvoiceLineItem.Remove(lineItem);
            await _context.SaveChangesAsync();
        }
    }
}
