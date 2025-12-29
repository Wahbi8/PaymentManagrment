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
    public class PaymentMethodController : ControllerBase
    {
        private readonly PaymentMethodServices _paymentMethodServices;
        public PaymentMethodController(PaymentMethodServices paymentMethodServices)
        {
            _paymentMethodServices = paymentMethodServices;
        }

        [HttpGet]
        public async Task<List<PaymentMethod>> GetPaymentMethodByUserId()
        {
            return await _paymentMethodServices.GetPaymentMethodsByUserId(User.GetUserId());
        }

        [HttpPost]
        public async Task<IActionResult> AddPaymentMethod([FromBody] PaymentMethod paymentMethod)
        {
            await _paymentMethodServices.AddPaymentMethod(paymentMethod);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePaymentMethod([FromBody] PaymentMethod paymentMethod)
        {
            await _paymentMethodServices.UpdatePaymentMethod(paymentMethod);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeletePaymentMethod([FromBody] Guid id)
        {
            await _paymentMethodServices.DeletePaymentMethod(id);
            return Ok();
        }

        [HttpGet]
        public async Task<PaymentMethod> GetPaymentMethodById([FromBody] Guid id)
        {
            return await _paymentMethodServices.GetPaymentMethodById(id);
        }
    }
}
