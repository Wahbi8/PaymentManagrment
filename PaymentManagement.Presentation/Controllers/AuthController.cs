using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentManagement.Domain;
using PaymentManagement.Application.Services;

namespace PaymentManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly AuthServices _authService;

        public AuthController(AuthServices authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<ActionResult> Register(User? user)
        {
            var res = await _authService.Register(user);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult> Login(string email, string password)
        {
            string res = await _authService.Login(email, password);

            return Ok(res);
        }
    }
}
