using Microsoft.Extensions.Configuration;
using Moq;
using PaymentManagement.Application.DTO;
using PaymentManagement.Application.Services;
using PaymentManagement.Domain;
using PaymentManagement.Domain.Interfaces;
using PaymentManagement.Domain.models;
using Xunit;

namespace PaymentManagement.Tests
{
    public class AuthServicesTests
    {
        private readonly Mock<IAuthRepository> _mockAuthRepository;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly AuthServices _authServices;

        public AuthServicesTests()
        {
            _mockAuthRepository = new Mock<IAuthRepository>();
            _mockConfiguration = new Mock<IConfiguration>();
            
            var jwtSection = new Mock<IConfigurationSection>();
            jwtSection.Setup(x => x["SecretKey"]).Returns("ThisIsASecretKeyForJwtTokenGeneration12345");
            jwtSection.Setup(x => x["Issuer"]).Returns("TestIssuer");
            jwtSection.Setup(x => x["Audience"]).Returns("TestAudience");
            _mockConfiguration.Setup(x => x.GetSection("JwtSettings")).Returns(jwtSection.Object);

            _authServices = new AuthServices(_mockAuthRepository.Object, _mockConfiguration.Object);
        }

        [Fact]
        public async Task Register_HashesPassword()
        {
            var user = new User { Name = "John", Email = "john@test.com", Password = "password", CompanyId = Guid.NewGuid() };
            _mockAuthRepository.Setup(x => x.Register(It.IsAny<User>())).Returns(Task.CompletedTask);

            await _authServices.Register(user);

            Assert.NotEqual("password", user.Password);
            _mockAuthRepository.Verify(x => x.Register(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Register_ThrowsException_WhenUserIsNull()
        {
            await Assert.ThrowsAsync<BusinessException>(() => _authServices.Register(null!));
        }

        [Fact]
        public async Task Register_ThrowsException_WhenFieldsAreMissing()
        {
            var user = new User { Name = "John", Email = null, Password = "password", CompanyId = Guid.NewGuid() };

            await Assert.ThrowsAsync<BusinessException>(() => _authServices.Register(user));
        }

        [Fact]
        public async Task Login_ReturnsTokens_WhenCredentialsAreValid()
        {
            var user = new User { Id = Guid.NewGuid(), Name = "John", Email = "john@test.com", Password = BCrypt.Net.BCrypt.HashPassword("password"), CompanyId = Guid.NewGuid() };
            _mockAuthRepository.Setup(x => x.LogIn("john@test.com", "password")).ReturnsAsync(user);
            _mockAuthRepository.Setup(x => x.AddRefreshToken(It.IsAny<RefreshToken>())).Returns(Task.CompletedTask);

            var result = await _authServices.Login("john@test.com", "password");

            Assert.NotNull(result);
            Assert.NotNull(result.AccessToken);
            Assert.NotNull(result.RefreshToken);
        }

        [Fact]
        public async Task Login_ThrowsException_WhenCredentialsAreInvalid()
        {
            _mockAuthRepository.Setup(x => x.LogIn("john@test.com", "wrongpassword")).ReturnsAsync((User?)null);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authServices.Login("john@test.com", "wrongpassword"));
        }

        [Fact]
        public async Task Login_ThrowsException_WhenEmailIsNull()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _authServices.Login(null!, "password"));
        }

        [Fact]
        public void GenerateAccessToken_ReturnsValidToken()
        {
            var userId = Guid.NewGuid().ToString();
            var email = "john@test.com";

            var token = _authServices.GenerateAccessToken(userId, email);

            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        [Fact]
        public void GenerateRefreshToken_ReturnsValidToken()
        {
            var refreshToken = _authServices.GenerateRefreshToken();

            Assert.NotNull(refreshToken);
            Assert.NotEmpty(refreshToken.Token);
            Assert.True(refreshToken.ExpireDate > DateTime.UtcNow);
            Assert.False(refreshToken.IsUsed);
            Assert.False(refreshToken.IsRevoked);
        }
    }
}
