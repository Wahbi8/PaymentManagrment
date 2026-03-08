using Microsoft.EntityFrameworkCore;
using PaymentManagement.Domain;
using PaymentManagement.Domain.Interfaces;

namespace PaymentManagement.Infrastructure
{
    public class AuditTrailRepository : IAuditTrailRepository
    {
        private readonly AppDbContext _context;

        public AuditTrailRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AuditTrail> GetById(Guid id)
            => await _context.AuditTrail.FindAsync(id) 
               ?? throw new BusinessException("Audit trail not found");

        public async Task<List<AuditTrail>> GetByEntityId(Guid entityId)
            => await _context.AuditTrail.Where(x => x.EntityId == entityId)
                .OrderByDescending(x => x.CreatedAt).ToListAsync();

        public async Task<List<AuditTrail>> GetByUserId(Guid userId)
            => await _context.AuditTrail.Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt).ToListAsync();

        public async Task<List<AuditTrail>> GetByDateRange(DateTime startDate, DateTime endDate)
            => await _context.AuditTrail.Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .OrderByDescending(x => x.CreatedAt).ToListAsync();

        public async Task Add(AuditTrail auditTrail)
        {
            await _context.AuditTrail.AddAsync(auditTrail);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AuditTrail>> GetAll(int pageNumber, int pageSize)
            => await _context.AuditTrail
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
    }
}
