using PaymentManagement.Domain;

namespace PaymentManagement.Domain.Interfaces
{
    public interface IAuditTrailRepository
    {
        Task<AuditTrail> GetById(Guid id);
        Task<List<AuditTrail>> GetByEntityId(Guid entityId);
        Task<List<AuditTrail>> GetByUserId(Guid userId);
        Task<List<AuditTrail>> GetByDateRange(DateTime startDate, DateTime endDate);
        Task Add(AuditTrail auditTrail);
        Task<List<AuditTrail>> GetAll(int pageNumber, int pageSize);
    }
}
