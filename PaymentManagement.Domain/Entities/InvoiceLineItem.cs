using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentManagement.Domain
{
    [Table("invoice_line_item")]
    public class InvoiceLineItem
    {
        [Column("id")]
        public Guid Id { get; set; }
        
        [Column("invoice_id")]
        public Guid InvoiceId { get; set; }
        
        [Column("description")]
        public string Description { get; set; } = string.Empty;
        
        [Column("quantity")]
        public decimal Quantity { get; set; }
        
        [Column("unit_price")]
        public decimal UnitPrice { get; set; }
        
        [Column("tax_rate")]
        public decimal TaxRate { get; set; }
        
        [Column("tax_amount")]
        public decimal TaxAmount { get; set; }
        
        [Column("subtotal")]
        public decimal Subtotal { get; set; }
        
        [Column("total")]
        public decimal Total { get; set; }
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [NotMapped]
        public Invoice? Invoice { get; set; }
        
        public void CalculateTotals()
        {
            Subtotal = Quantity * UnitPrice;
            TaxAmount = Subtotal * (TaxRate / 100);
            Total = Subtotal + TaxAmount;
        }
    }
}
