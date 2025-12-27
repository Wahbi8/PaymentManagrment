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
            if (users == null || !users.Any())
                throw new BusinessException("No users found");
            return users;
        }

        public async Task AddUser(User user)
        {
            if (user == null)
                throw new BusinessException($"{nameof(User)} cannot be empty");

            user.Id = Guid.NewGuid();
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            await _userRepository.AddUser(user);
        }

        public async Task<User?> GetUserById(Guid id)
        {
            if (id == Guid.Empty)
                throw new BusinessException("UserId is empty");

            User? user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            return user;
        }
    }
}
