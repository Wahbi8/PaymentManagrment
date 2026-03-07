using AutoMapper;
using Moq;
using PaymentManagement.Application.DTO;
using PaymentManagement.Domain.Interfaces;
using PaymentManagement.Application.Mappings;
using PaymentManagement.Application.Services;
using PaymentManagement.Domain;
using Xunit;

namespace PaymentManagement.Tests
{
    public class UserServicesTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly IMapper _mapper;
        private readonly UserServices _userServices;

        public UserServicesTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            _userServices = new UserServices(_mockUserRepository.Object);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsListOfUsers()
        {
            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), Name = "John", Email = "john@test.com", Password = "pass", CompanyId = Guid.NewGuid() },
                new User { Id = Guid.NewGuid(), Name = "Jane", Email = "jane@test.com", Password = "pass", CompanyId = Guid.NewGuid() }
            };
            _mockUserRepository.Setup(x => x.GetAllUsers()).ReturnsAsync(users);

            var result = await _userServices.GetAllUsers();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllUsers_ThrowsException_WhenNoUsers()
        {
            _mockUserRepository.Setup(x => x.GetAllUsers()).ReturnsAsync(new List<User>());

            await Assert.ThrowsAsync<BusinessException>(() => _userServices.GetAllUsers());
        }

        [Fact]
        public async Task GetUserById_ReturnsUser_WhenUserExists()
        {
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Name = "John", Email = "john@test.com", Password = "pass", CompanyId = Guid.NewGuid() };
            _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);

            var result = await _userServices.GetUserById(userId);

            Assert.NotNull(result);
            Assert.Equal("John", result.Name);
        }

        [Fact]
        public async Task GetUserById_ReturnsNull_WhenUserNotFound()
        {
            var userId = Guid.NewGuid();
            _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync((User?)null);

            var result = await _userServices.GetUserById(userId);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserById_ThrowsException_WhenIdIsEmpty()
        {
            await Assert.ThrowsAsync<BusinessException>(() => _userServices.GetUserById(Guid.Empty));
        }

        [Fact]
        public async Task AddUser_SetsIdAndHashesPassword()
        {
            var user = new User { Name = "John", Email = "john@test.com", Password = "password", CompanyId = Guid.NewGuid() };
            _mockUserRepository.Setup(x => x.AddUser(It.IsAny<User>())).Returns(Task.CompletedTask);

            await _userServices.AddUser(user);

            Assert.NotEqual(Guid.Empty, user.Id);
            Assert.NotEqual("password", user.Password);
            _mockUserRepository.Verify(x => x.AddUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task AddUser_ThrowsException_WhenUserIsNull()
        {
            await Assert.ThrowsAsync<BusinessException>(() => _userServices.AddUser(null!));
        }
    }
}
