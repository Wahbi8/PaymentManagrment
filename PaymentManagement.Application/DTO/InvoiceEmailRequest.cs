using PaymentManagement.Domain;

namespace PaymentManagement.Application.DTO
{
    public class InvoiceEmailRequest
    {
        public Guid InvoiceId { get; set; }
        public string RecipientEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public InvoiceStatus InvoiceType { get; set; }

        public InvoiceEmailRequest(Guid invoiceId, string recipientEmail)
        {
            InvoiceId = invoiceId == Guid.Empty ? 
                throw new DomainException("InvoiceId is empty") : invoiceId ;
            RecipientEmail = string.IsNullOrEmpty(recipientEmail) ?
                throw new DomainException("the recipient email is empty") : recipientEmail;
        } 
        private InvoiceEmailRequest() { }
    }
}
