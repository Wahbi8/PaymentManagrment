using PaymentManagement.Domain;
using PaymentManagement.Domain.models;

namespace PaymentManagement.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task Register(User user);
        Task<User?> LogIn(string email, string password);
        Task AddRefreshToken(RefreshToken refreshToken);
    }
}
