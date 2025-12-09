using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentManagement.Domain
{
    [Table("invoice")]
    public class Invoice
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("company_id")]
        public Guid CompanyId { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [Column("issue_date")]
        public DateTime IssueDate { get; set; }
        [Column("due_date")]
        public DateTime DueDate { get; set; }
        [Column("total_amount")]
        public Decimal TotalAmount { get; set; }
        [Column("amount_paid")]
        public decimal AmountPaid { get; set; }
        [Column("is_paid")]
        public bool IsPaid { get; set; }
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }
        [Column("created_by")]
        public Guid CreatedBy { get; set; }
        [Column("updated_by")]
        public Guid UpdatedBy { get; set; }
    }
}
