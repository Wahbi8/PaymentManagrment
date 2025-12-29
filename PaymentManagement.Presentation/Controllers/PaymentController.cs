using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PaymentManagement.Domain;
using PaymentManagement.Application.Services;
using PaymentManagement.Presentation.Common.Extensions;

namespace PaymentManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentServices _paymentServices;
        public PaymentController(PaymentServices paymentServices)
        {
            _paymentServices = paymentServices;
        }

        [HttpGet]
        public async Task<List<Payment>> GetPaymentsByInvoiceId(Guid id)
        {
            return await _paymentServices.GetPaymentsByInvoiceId(id);
        }

        [HttpGet]
        public async Task<List<Payment>> GetAllPaymentsByUserId()
        {
            return await _paymentServices.GetAllPaymentsByUserId(User.GetUserId());
        }

        [HttpPost]
        public async Task<IActionResult> AddPayment([FromBody] Payment payment)
        {
            await _paymentServices.AddPayment(payment);
            return Ok();
        }
    }
}
