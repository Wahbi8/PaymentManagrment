using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PaymentManagement.Application.DTO;
using PaymentManagement.Domain;
using PaymentManagement.Domain.models;
using PaymentManagement.Infrastructure;

namespace PaymentManagement.Application.Services
{
    public class AuthServices
    {
        private readonly AuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthServices(AuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration.GetRequiredSection("JwtSettings");
        }
        public async Task Register(User? user)
        {
            if (user == null) 
                throw new BusinessException("fill the fields befor submiting");
            if (user.Name == null || user.Password == null || user.Email == null)
                throw new BusinessException("Fill all the fields befor submiting");

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            await _authRepository.Register(user);
        }

        public async Task<LoginResponse> Login(string email, string password)
        {
            if (password == null || email == null)
                throw new ArgumentException("Fill all the fields befor submiting");

            var user = await _authRepository.LogIn(email, password);

            if (user == null) throw new UnauthorizedAccessException("Invalid email or password");

            string accessToken = GenerateAccessToken(user.Id.ToString(), user.Email);

            var refreshToken = GenerateRefreshToken();
            refreshToken.UserId = user.Id;

            await _authRepository.AddRefreshToken(refreshToken);

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                AccessTokenExpires = DateTime.UtcNow.AddMinutes(30),
                RefreshTokenExpires = refreshToken.ExpireDate
            };
        }

        public string GenerateAccessToken(string userId, string email)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId), // Subject (User ID)
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique ID for this specific token
                // Add "Role" claims here if needed (e.g., "Admin", "User")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Issuer"],
                audience: _configuration["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15), // CHANGE: Reduced from 1 hour to 15 mins
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpireDate = DateTime.UtcNow.AddDays(7),
                AddedDate = DateTime.UtcNow,
                IsUsed = false,
                IsRevoked = false
            };
        }

    }
}
