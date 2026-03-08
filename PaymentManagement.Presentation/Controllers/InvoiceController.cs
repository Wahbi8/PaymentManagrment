using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PaymentManagement.Domain;
using PaymentManagement.Application.Services;
using PaymentManagement.Presentation.Common.Extensions;
using PaymentManagement.Application.DTO;

namespace PaymentManagement.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceServices _invoiceServices;

        public InvoiceController(InvoiceServices invoiceServices)
        {
            _invoiceServices = invoiceServices;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Invoice>>> GetInvoicesByUserId([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var userId = User.GetUserId();
            var result = await _invoiceServices.GetInvoicesByUserId(userId, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoiceById(Guid id)
        {
            var invoice = await _invoiceServices.GetInvoiceById(id);
            return Ok(invoice);
        }

        [HttpPost]
        public async Task<ActionResult<Invoice>> CreateInvoiceWithLineItems([FromBody] CreateInvoiceWithLineItemsDto dto)
        {
            var userId = User.GetUserId();
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "unknown";
            
            var invoice = await _invoiceServices.CreateInvoiceWithLineItems(dto, userId, userEmail);
            return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.Id }, invoice);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(Guid id, [FromBody] Invoice invoice)
        {
            var userId = User.GetUserId();
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "unknown";
            
            invoice.Id = id;
            await _invoiceServices.UpdateInvoice(invoice, userId, userEmail);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(Guid id)
        {
            var userId = User.GetUserId();
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "unknown";
            
            await _invoiceServices.DeleteInvoice(id, userId, userEmail);
            return NoContent();
        }

        [HttpPost("{id}/send")]
        public async Task<IActionResult> SendInvoice(Guid id)
        {
            var userId = User.GetUserId();
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "unknown";
            
            await _invoiceServices.SendInvoice(id, userId, userEmail);
            return Ok();
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelInvoice(Guid id)
        {
            var userId = User.GetUserId();
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "unknown";
            
            await _invoiceServices.CancelInvoice(id, userId, userEmail);
            return Ok();
        }

        [HttpGet("all")]
        public async Task<ActionResult<PagedResult<Invoice>>> GetAllInvoices([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _invoiceServices.GetAllInvoices(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpPost("{id}/line-items")]
        public async Task<IActionResult> AddLineItem(Guid id, [FromBody] CreateInvoiceLineItemDto dto)
        {
            await _invoiceServices.AddLineItem(id, dto);
            return Ok();
        }
    }
}
