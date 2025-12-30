using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PaymentManagement.Domain;
using PaymentManagement.Application.Services;
using PaymentManagement.Presentation.Common.Extensions;
using PaymentManagement.Application.DTO;

namespace PaymentManagement.Presentation.Controllers
{
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceServices _invoiceServices;
        public InvoiceController(InvoiceServices invoiceServices)
        {
            _invoiceServices = invoiceServices;
        }
        [HttpGet]
        public async Task<List<Invoice>> GetInvoicesByCompanyId()
        {
            return await _invoiceServices.GetInvoicesByCompanyId(User.GetUserId());
        }
        [HttpGet]
        public async Task<Invoice> GetInvoiceById(Guid id)
        {
            return await _invoiceServices.GetInvoiceById(id);
        }
        [HttpPost]
        public async Task<IActionResult> AddInvoice([FromBody] Invoice invoice)
        {
            await _invoiceServices.AddInvoice(invoice);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateInvoice([FromBody] Invoice invoice)
        {
            await _invoiceServices.UpdateInvoice(invoice);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(Guid id)
        {
            await _invoiceServices.DeleteInvoice(id);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SendInvoiceEmail([FromBody] InvoiceEmailRequest request)
        {
            await _invoiceServices.SendInvoiceEmail(request.InvoiceId, request.RecipientEmail);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CancelInvoice(Guid id)
        {
            await _invoiceServices.CancelInvoice(id);
            return Ok();
        }
    }
}
