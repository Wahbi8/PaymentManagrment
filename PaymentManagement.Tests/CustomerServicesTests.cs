using Moq;
using PaymentManagement.Domain.Interfaces;
using PaymentManagement.Application.Services;
using PaymentManagement.Domain;
using Xunit;

namespace PaymentManagement.Tests
{
    public class CustomerServicesTests
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly CustomerServices _customerServices;

        public CustomerServicesTests()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _customerServices = new CustomerServices(_mockCustomerRepository.Object);
        }

        [Fact]
        public async Task GetCustomerById_ReturnsCustomer_WhenCustomerExists()
        {
            var customerId = Guid.NewGuid();
            var customer = new Customer { Id = customerId, Name = "John", Email = "john@test.com", CompanyId = Guid.NewGuid() };
            _mockCustomerRepository.Setup(x => x.GetCustomerById(customerId)).ReturnsAsync(customer);

            var result = await _customerServices.GetCustomerById(customerId);

            Assert.NotNull(result);
            Assert.Equal("John", result.Name);
        }

        [Fact]
        public async Task GetCustomerById_ThrowsException_WhenCustomerNotFound()
        {
            var customerId = Guid.NewGuid();
            _mockCustomerRepository.Setup(x => x.GetCustomerById(customerId)).ReturnsAsync((Customer?)null);

            await Assert.ThrowsAsync<BusinessException>(() => _customerServices.GetCustomerById(customerId));
        }

        [Fact]
        public async Task GetCustomerById_ThrowsException_WhenIdIsEmpty()
        {
            await Assert.ThrowsAsync<BusinessException>(() => _customerServices.GetCustomerById(Guid.Empty));
        }

        [Fact]
        public async Task GetAllCustomers_ReturnsListOfCustomers()
        {
            var customers = new List<Customer>
            {
                new Customer { Id = Guid.NewGuid(), Name = "John", Email = "john@test.com", CompanyId = Guid.NewGuid() },
                new Customer { Id = Guid.NewGuid(), Name = "Jane", Email = "jane@test.com", CompanyId = Guid.NewGuid() }
            };
            _mockCustomerRepository.Setup(x => x.GetAllCustomers()).ReturnsAsync(customers);

            var result = await _customerServices.GetAllCustomers();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllCustomers_ThrowsException_WhenNoCustomers()
        {
            _mockCustomerRepository.Setup(x => x.GetAllCustomers()).ReturnsAsync(new List<Customer>());

            await Assert.ThrowsAsync<BusinessException>(() => _customerServices.GetAllCustomers());
        }

        [Fact]
        public async Task AddCustomer_SetsId()
        {
            var customer = new Customer { Name = "John", Email = "john@test.com", CompanyId = Guid.NewGuid() };
            _mockCustomerRepository.Setup(x => x.AddCustomer(It.IsAny<Customer>())).Returns(Task.CompletedTask);

            await _customerServices.AddCustomer(customer);

            Assert.NotEqual(Guid.Empty, customer.Id);
            _mockCustomerRepository.Verify(x => x.AddCustomer(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task AddCustomer_ThrowsException_WhenCustomerIsNull()
        {
            await Assert.ThrowsAsync<BusinessException>(() => _customerServices.AddCustomer(null!));
        }

        [Fact]
        public async Task DeleteCustomer_CallsRepository()
        {
            var customerId = Guid.NewGuid();
            _mockCustomerRepository.Setup(x => x.DeleteCustomer(customerId)).Returns(Task.CompletedTask);

            await _customerServices.DeleteCustomer(customerId);

            _mockCustomerRepository.Verify(x => x.DeleteCustomer(customerId), Times.Once);
        }

        [Fact]
        public async Task DeleteCustomer_ThrowsException_WhenIdIsEmpty()
        {
            await Assert.ThrowsAsync<BusinessException>(() => _customerServices.DeleteCustomer(Guid.Empty));
        }
    }
}
