using PaymentManagement.Domain;
using PaymentManagement.Infrastructure;

namespace PaymentManagement.Application.Services
{
    public class UserServices
    {
        private readonly UserRepository _userRepository;

        public UserServices(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetAllUsers()
        {
            List<User> users = await _userRepository.GetAllUsers();
            return users;
        }

        public async Task<bool> AddUser(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            bool res = await _userRepository.AddUser(user);
            return res;
        }

        public async Task<User?> GetUserById(Guid id)
        {
            User? user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            return user;
        }
    }
}
