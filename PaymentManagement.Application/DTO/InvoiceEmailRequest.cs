using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentManagement.Application.DTO
{
    public class InvoiceEmailRequest
    {
        public Guid InvoiceId { get; set; }
        public string RecipientEmail { get; set; }
    }
}
