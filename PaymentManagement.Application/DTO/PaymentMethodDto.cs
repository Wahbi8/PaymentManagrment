namespace PaymentManagement.Application.DTO
{
    public class PaymentMethodDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public int Type { get; set; }
        public string PaymentToken { get; set; } = string.Empty;
        public string CardBrand { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreatePaymentMethodDto
    {
        public Guid CustomerId { get; set; }
        public int Type { get; set; }
        public string PaymentToken { get; set; } = string.Empty;
        public string CardBrand { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
    }
}
