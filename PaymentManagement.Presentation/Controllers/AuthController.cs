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
        public async Task Register(User? user)
        {
            await _authService.Register(user);
        }

        [HttpPost]
        public async Task Login(string email, string password)
        {
            await _authService.Login(email, password);
        }
    }
}
