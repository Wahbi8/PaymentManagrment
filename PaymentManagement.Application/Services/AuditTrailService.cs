using System.Text.Json;
using PaymentManagement.Domain;
using PaymentManagement.Domain.Interfaces;

namespace PaymentManagement.Application.Services
{
    public class AuditTrailService
    {
        private readonly IAuditTrailRepository _auditTrailRepository;

        public AuditTrailService(IAuditTrailRepository auditTrailRepository)
        {
            _auditTrailRepository = auditTrailRepository;
        }

        public async Task LogAsync(string entityName, Guid entityId, string action, object? oldValues = null, object? newValues = null, Guid? userId = null, string? userEmail = null, string? ipAddress = null)
        {
            var auditTrail = new AuditTrail
            {
                Id = Guid.NewGuid(),
                EntityName = entityName,
                EntityId = entityId,
                Action = action,
                OldValues = oldValues != null ? JsonSerializer.Serialize(oldValues) : null,
                NewValues = newValues != null ? JsonSerializer.Serialize(newValues) : null,
                UserId = userId,
                UserEmail = userEmail,
                IpAddress = ipAddress,
                CreatedAt = DateTime.UtcNow
            };

            await _auditTrailRepository.Add(auditTrail);
        }

        public async Task LogCreateAsync(string entityName, Guid entityId, object newValues, Guid? userId = null, string? userEmail = null)
        {
            await LogAsync(entityName, entityId, AuditAction.Created.ToString(), null, newValues, userId, userEmail);
        }

        public async Task LogUpdateAsync(string entityName, Guid entityId, object oldValues, object newValues, Guid? userId = null, string? userEmail = null)
        {
            await LogAsync(entityName, entityId, AuditAction.Updated.ToString(), oldValues, newValues, userId, userEmail);
        }

        public async Task LogDeleteAsync(string entityName, Guid entityId, object oldValues, Guid? userId = null, string? userEmail = null)
        {
            await LogAsync(entityName, entityId, AuditAction.Deleted.ToString(), oldValues, null, userId, userEmail);
        }

        public async Task<List<AuditTrail>> GetByEntityIdAsync(Guid entityId)
        {
            return await _auditTrailRepository.GetByEntityId(entityId);
        }

        public async Task<List<AuditTrail>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _auditTrailRepository.GetAll(pageNumber, pageSize);
        }
    }
}
