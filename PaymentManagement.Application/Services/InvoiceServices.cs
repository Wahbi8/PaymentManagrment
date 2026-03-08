using AutoMapper;
using Org.BouncyCastle.Utilities;
using PaymentManagement.Application.DTO;
using PaymentManagement.Domain;
using PaymentManagement.Domain.Interfaces;

namespace PaymentManagement.Application.Services
{
    public class InvoiceServices
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceLineItemRepository _lineItemRepository;
        private readonly IAuditTrailRepository _auditTrailRepository;
        private readonly IMapper _mapper;

        public InvoiceServices(
            IInvoiceRepository invoiceRepository,
            IInvoiceLineItemRepository lineItemRepository,
            IAuditTrailRepository auditTrailRepository,
            IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _lineItemRepository = lineItemRepository;
            _auditTrailRepository = auditTrailRepository;
            _mapper = mapper;
        }

        public async Task<Invoice> GetInvoiceById(Guid id)
        {
            if (id == Guid.Empty)
                throw new BusinessException("Invoice id cannot be empty");
            
            var invoice = await _invoiceRepository.GetInvoiceById(id);
            if (invoice == null)
                throw new BusinessException("Invoice not found");
            
            invoice.LineItems = await _lineItemRepository.GetByInvoiceId(id);
            return invoice;
        }

        public async Task<PagedResult<Invoice>> GetInvoicesByUserId(Guid userId, int pageNumber = 1, int pageSize = 10)
        {
            if (userId == Guid.Empty)
                throw new BusinessException("User id cannot be empty");

            var allInvoices = await _invoiceRepository.GetInvoicesByUserId(userId, pageNumber, pageSize);
            var totalCount = await _invoiceRepository.GetInvoicesCountByUserId(userId);

            return new PagedResult<Invoice>
            {
                Items = allInvoices,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<Invoice>> GetAllInvoices(int pageNumber = 1, int pageSize = 10)
        {
            var items = await _invoiceRepository.GetAllInvoices(pageNumber, pageSize);
            var totalCount = await _invoiceRepository.GetInvoicesCount();

            return new PagedResult<Invoice>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Invoice> CreateInvoiceWithLineItems(CreateInvoiceWithLineItemsDto dto, Guid userId, string userEmail)
        {
            if (dto == null)
                throw new BusinessException("Invoice data cannot be empty");

            var invoice = new Invoice
            {
                Id = Guid.NewGuid(),
                CompanyId = dto.CompanyId,
                UserId = userId,
                IssueDate = dto.IssueDate,
                DueDate = dto.DueDate,
                Status = InvoiceStatus.draft,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userId,
                UpdatedBy = userId,
                LineItems = new List<InvoiceLineItem>()
            };

            foreach (var itemDto in dto.LineItems)
            {
                var lineItem = new InvoiceLineItem
                {
                    Id = Guid.NewGuid(),
                    InvoiceId = invoice.Id,
                    Description = itemDto.Description,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice,
                    TaxRate = itemDto.TaxRate,
                    CreatedAt = DateTime.UtcNow
                };
                lineItem.CalculateTotals();
                invoice.LineItems.Add(lineItem);
            }

            invoice.RecalculateTotals();
            invoice.BalanceDue = invoice.TotalAmount;

            await _invoiceRepository.AddInvoice(invoice);
            await _lineItemRepository.AddRange(invoice.LineItems);

            var auditTrail = new AuditTrail
            {
                Id = Guid.NewGuid(),
                EntityName = nameof(Invoice),
                EntityId = invoice.Id,
                Action = AuditAction.Created.ToString(),
                NewValues = System.Text.Json.JsonSerializer.Serialize(_mapper.Map<InvoiceDto>(invoice)),
                UserId = userId,
                UserEmail = userEmail,
                CreatedAt = DateTime.UtcNow
            };
            await _auditTrailRepository.Add(auditTrail);

            return invoice;
        }

        public async Task UpdateInvoice(Invoice invoice, Guid userId, string userEmail)
        {
            if (invoice == null)
                throw new BusinessException($"{nameof(Invoice)} cannot be empty");

            if (invoice.Status != InvoiceStatus.draft)
                throw new BusinessException("Only draft invoices can be updated.");

            var oldInvoice = await _invoiceRepository.GetInvoiceById(invoice.Id);
            invoice.UpdatedAt = DateTime.UtcNow;
            invoice.UpdatedBy = userId;
            
            await _invoiceRepository.UpdateInvoice(invoice);

            var auditTrail = new AuditTrail
            {
                Id = Guid.NewGuid(),
                EntityName = nameof(Invoice),
                EntityId = invoice.Id,
                Action = AuditAction.Updated.ToString(),
                OldValues = System.Text.Json.JsonSerializer.Serialize(_mapper.Map<InvoiceDto>(oldInvoice)),
                NewValues = System.Text.Json.JsonSerializer.Serialize(_mapper.Map<InvoiceDto>(invoice)),
                UserId = userId,
                UserEmail = userEmail,
                CreatedAt = DateTime.UtcNow
            };
            await _auditTrailRepository.Add(auditTrail);
        }

        public async Task DeleteInvoice(Guid id, Guid userId, string userEmail)
        {
            if (id == Guid.Empty)
                throw new BusinessException("Invoice id cannot be empty");

            var invoice = await _invoiceRepository.GetInvoiceById(id);
            if (invoice.Status != InvoiceStatus.draft)
                throw new BusinessException("Only draft invoices can be deleted.");

            var lineItems = await _lineItemRepository.GetByInvoiceId(id);
            
            await _invoiceRepository.DeleteInvoice(id);

            var auditTrail = new AuditTrail
            {
                Id = Guid.NewGuid(),
                EntityName = nameof(Invoice),
                EntityId = id,
                Action = AuditAction.Deleted.ToString(),
                OldValues = System.Text.Json.JsonSerializer.Serialize(_mapper.Map<InvoiceDto>(invoice)),
                UserId = userId,
                UserEmail = userEmail,
                CreatedAt = DateTime.UtcNow
            };
            await _auditTrailRepository.Add(auditTrail);
        }

        public async Task SendInvoice(Guid invoiceId, Guid userId, string userEmail)
        {
            if (invoiceId == Guid.Empty)
                throw new BusinessException("Invoice id cannot be empty");
            
            var invoice = await _invoiceRepository.GetInvoiceById(invoiceId);
            if (invoice == null)
                throw new BusinessException("Invoice not found");

            if (!invoice.LineItems.Any())
                invoice.LineItems = await _lineItemRepository.GetByInvoiceId(invoiceId);

            invoice.send();
            invoice.UpdatedAt = DateTime.UtcNow;
            invoice.UpdatedBy = userId;
            
            await _invoiceRepository.UpdateInvoice(invoice);

            var auditTrail = new AuditTrail
            {
                Id = Guid.NewGuid(),
                EntityName = nameof(Invoice),
                EntityId = invoice.Id,
                Action = AuditAction.Sent.ToString(),
                UserId = userId,
                UserEmail = userEmail,
                CreatedAt = DateTime.UtcNow
            };
            await _auditTrailRepository.Add(auditTrail);
        }

        public async Task CancelInvoice(Guid id, Guid userId, string userEmail)
        {
            if (id == Guid.Empty)
                throw new BusinessException("Invoice id cannot be empty");
            
            var invoice = await _invoiceRepository.GetInvoiceById(id);
            if (invoice == null)
                throw new BusinessException("Invoice not found");
            
            invoice.cancel();
            invoice.UpdatedAt = DateTime.UtcNow;
            invoice.UpdatedBy = userId;
            
            await _invoiceRepository.UpdateInvoice(invoice);

            var auditTrail = new AuditTrail
            {
                Id = Guid.NewGuid(),
                EntityName = nameof(Invoice),
                EntityId = invoice.Id,
                Action = AuditAction.Cancelled.ToString(),
                UserId = userId,
                UserEmail = userEmail,
                CreatedAt = DateTime.UtcNow
            };
            await _auditTrailRepository.Add(auditTrail);
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
                await _invoiceRepository.UpdateInvoice(invoice);
            }
        }

        public async Task AddLineItem(Guid invoiceId, CreateInvoiceLineItemDto dto)
        {
            if (invoiceId == Guid.Empty)
                throw new BusinessException("Invoice id cannot be empty");
            
            var invoice = await _invoiceRepository.GetInvoiceById(invoiceId);
            if (invoice == null)
                throw new BusinessException("Invoice not found");

            if (invoice.Status != InvoiceStatus.draft)
                throw new BusinessException("Can only add line items to draft invoices.");

            var lineItem = new InvoiceLineItem
            {
                Id = Guid.NewGuid(),
                InvoiceId = invoiceId,
                Description = dto.Description,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,
                TaxRate = dto.TaxRate,
                CreatedAt = DateTime.UtcNow
            };
            lineItem.CalculateTotals();

            await _lineItemRepository.Add(lineItem);
            
            invoice.LineItems = await _lineItemRepository.GetByInvoiceId(invoiceId);
            invoice.LineItems.Add(lineItem);
            invoice.RecalculateTotals();
            invoice.BalanceDue = invoice.TotalAmount - invoice.AmountPaid;
            
            await _invoiceRepository.UpdateInvoice(invoice);
        }
    }
}
