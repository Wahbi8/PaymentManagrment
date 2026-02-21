using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentManagement.Application.Services;
using PaymentManagement.Domain;
using PaymentManagement.Infrastructure;

namespace PaymentManagement.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly UserServices _userServices;
        public UserController(UserRepository userRepository, UserServices userServices)
        {
            _userRepository = userRepository;
            _userServices = userServices;
        }

        [HttpGet]
        public async Task<ActionResult<User>> GetAllUsers()
        {
            //User user = new User();
            List<User> users = await _userServices.GetAllUsers();
            return Ok(users);

            //var users = await _userRepository.GetAllUsers();
            //return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult> AddUser(User user)
        {
            await _userServices.AddUser(user);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<User>> GetUserById(Guid id)
        {
            User user = await _userServices.GetUserById(id);   
            return Ok(user);
        
        }
    }
}
