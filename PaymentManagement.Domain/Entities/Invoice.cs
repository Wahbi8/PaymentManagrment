using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentManagement.Domain
{
    [Table("invoice")]
    public class Invoice
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("user_id")]
        public Guid UserId { get; set; }
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
        
        [Column("subtotal")]
        public decimal Subtotal { get; set; }
        
        [Column("tax_amount")]
        public decimal TaxAmount { get; set; }
        
        [Column("total_amount")]
        public decimal TotalAmount { get; set; }
        
        [Column("amount_paid")]
        public decimal AmountPaid { get; set; }
        
        [Column("balance_due")]
        public decimal BalanceDue { get; set; }
        
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
        
        [NotMapped]
        public List<InvoiceLineItem> LineItems { get; set; } = new();

        public void AddLineItem(InvoiceLineItem lineItem)
        {
            if (Status != InvoiceStatus.draft)
                throw new InvalidOperationException("Cannot add line items to a non-draft invoice.");
            
            lineItem.InvoiceId = Id;
            lineItem.CalculateTotals();
            LineItems.Add(lineItem);
            RecalculateTotals();
        }

        public void RecalculateTotals()
        {
            Subtotal = LineItems.Sum(x => x.Subtotal);
            TaxAmount = LineItems.Sum(x => x.TaxAmount);
            TotalAmount = LineItems.Sum(x => x.Total);
            BalanceDue = TotalAmount - AmountPaid;
        }

        public void send()
        {
            if (Status != InvoiceStatus.draft)
                throw new InvalidOperationException("Only draft invoices can be sent.");
            
            if (!LineItems.Any())
                throw new InvalidOperationException("Cannot send an invoice without line items.");

            Status = InvoiceStatus.sent;
        }

        public void markAsPaid()
        {
            if (Status != InvoiceStatus.sent && Status != InvoiceStatus.overdue && Status != InvoiceStatus.partiallyPaid)
                throw new InvalidOperationException("Only sent or partially paid invoices can be marked as paid.");

            Status = InvoiceStatus.paid;
            IsPaid = true;
            AmountPaid = TotalAmount;
            BalanceDue = 0;
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
            if (Status != InvoiceStatus.sent && Status != InvoiceStatus.overdue && Status != InvoiceStatus.partiallyPaid)
                throw new InvalidOperationException("Payment can only be applied to sent, overdue, or partially paid invoices.");

            if (amount <= 0)
                throw new InvalidOperationException("Payment amount must be greater than zero.");
                
            if (amount > BalanceDue)
                throw new InvalidOperationException($"Payment amount ({amount}) exceeds balance due ({BalanceDue}).");

            AmountPaid += amount;
            BalanceDue = TotalAmount - AmountPaid;
            
            if (BalanceDue <= 0)
            {
                IsPaid = true;
                Status = InvoiceStatus.paid;
                BalanceDue = 0;
            }
            else
            {
                Status = InvoiceStatus.partiallyPaid;
            }
        }
        
        public bool CanApplyPayment(decimal amount)
        {
            return Status == InvoiceStatus.sent || Status == InvoiceStatus.overdue || Status == InvoiceStatus.partiallyPaid
                   && amount > 0 && amount <= BalanceDue;
        }
    }

    public enum InvoiceStatus
    {
        draft = 0,
        sent,
        partiallyPaid,
        paid,
        overdue,
        cancelled
    }
}
