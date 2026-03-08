using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentManagement.Domain
{
    [Table("audit_trail")]
    public class AuditTrail
    {
        [Column("id")]
        public Guid Id { get; set; }
        
        [Column("entity_name")]
        public string EntityName { get; set; } = string.Empty;
        
        [Column("entity_id")]
        public Guid EntityId { get; set; }
        
        [Column("action")]
        public string Action { get; set; } = string.Empty;
        
        [Column("old_values")]
        public string? OldValues { get; set; }
        
        [Column("new_values")]
        public string? NewValues { get; set; }
        
        [Column("user_id")]
        public Guid? UserId { get; set; }
        
        [Column("user_email")]
        public string? UserEmail { get; set; }
        
        [Column("ip_address")]
        public string? IpAddress { get; set; }
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
    
    public enum AuditAction
    {
        Created,
        Updated,
        Deleted,
        Viewed,
        Paid,
        Cancelled,
        Sent,
        Refunded
    }
}
