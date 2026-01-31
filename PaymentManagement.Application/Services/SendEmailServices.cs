using System.Text;
using System.Text.Json;
using PaymentManagement.Application.DTO;
using PaymentManagement.Domain;

namespace PaymentManagement.Application.Services
{
    public class SendEmailServices
    {
        public async Task SendEmailAsync(InvoiceEmailRequest emailInfo)
        {
            var client = new HttpClient();

            string jsonData = JsonSerializer.Serialize(emailInfo);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var respose = await client.PostAsync("http//localhost:1212/email/invoice", content);
        }
    }
}
