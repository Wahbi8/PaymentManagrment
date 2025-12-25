using PaymentManagement.Domain;
using PaymentManagement.Domain.models;

namespace PaymentManagement.Infrastructure
{
    public class AuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Register(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> LogIn(string email, string password)
        {
            User? user = await _context.Users.FindAsync(email);
            
            if (user == null) return null;

            bool isvalid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            return isvalid ? user : null;
        }

        public async Task AddRefreshToken(RefreshToken refreshToken)
        {
            await _context.RefreshToken.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }
    }
}
