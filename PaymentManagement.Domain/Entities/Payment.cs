using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentManagement.Domain.Entities;

namespace PaymentManagement.Domain
{
    [Table("payment")]
    public class Payment
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("invoice_id")]
        public Guid InvoiceId { get; set; }
        [Column("amount")]
        public decimal Amount { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [Column("Deleted_at")]
        public DateTime DeletedAt { get; set; }
        [Column("created_by")]
        public Guid CreatedBy { get; set; }
        [Column("updated_by")]
        public Guid UpdatedBy { get; set; }
        [Column("deleted_by")]
        public Guid DeletedBy { get; set; }
        [Column("is_refunded")]
        public bool IsRefunded { get; set; }
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }
        [Column("currency")]
        public string Currency { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        [Column("payment_method_id")]
        public Guid? PaymentMethodId { get; set; }
    }
}
