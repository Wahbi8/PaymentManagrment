namespace PaymentManagement.Application.DTO
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public Guid InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsRefunded { get; set; }
        public bool IsDeleted { get; set; }
        public string Currency { get; set; } = string.Empty;
    }

    public class CreatePaymentDto
    {
        public Guid InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public CreatePaymentMethodDto? PaymentMethod { get; set; }
    }
}
