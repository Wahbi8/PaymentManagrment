using System.ComponentModel.DataAnnotations.Schema;

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

        [Column("status")]
        public InvoiceStatus Status { get; set; }

        public void send()
        {
            if (Status != InvoiceStatus.draft)
                throw new InvalidOperationException("Only draft invoices can be sent.");

            Status = InvoiceStatus.sent;
        }

        public void markAsPaid()
        {
            if (Status != InvoiceStatus.sent)
                throw new InvalidOperationException("Only sent invoices can be marked as paid.");

            Status = InvoiceStatus.paid;
            IsPaid = true;
            AmountPaid = TotalAmount;
        }

        public void cancel()
        {
            if (Status == InvoiceStatus.paid)
                throw new InvalidOperationException("Paid invoices cannot be cancelled.");
            Status = InvoiceStatus.cancelled;
        }

        public void markAsOverdue()
        {
            if (Status != InvoiceStatus.sent)
                throw new InvalidOperationException("Only sent invoices can be marked as overdue.");
            Status = InvoiceStatus.overdue;
        }

        public void ApplyPayment(decimal amount)
        {
            if (Status != InvoiceStatus.sent && Status != InvoiceStatus.overdue)
                throw new InvalidOperationException("Payment can only be applied to sent or underdoe invoices.");

            AmountPaid += amount;
            if (AmountPaid >= TotalAmount)
            {
                IsPaid = true;
                Status = InvoiceStatus.paid;
            }
        }
    }

    public enum InvoiceStatus
    {
        draft = 0,
        sent,
        paid,
        overdue,
        cancelled
    }
}
