using PaymentManagement.Domain;

namespace PaymentManagement.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();
        Task<User?> GetByIdAsync(Guid id);
        Task AddUser(User user);
        Task RemoveUser(Guid id);
        Task UpdateUser(User user);
    }
}
