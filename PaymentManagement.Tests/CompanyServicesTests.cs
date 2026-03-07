using Moq;
using PaymentManagement.Domain.Interfaces;
using PaymentManagement.Application.Services;
using PaymentManagement.Domain;
using Xunit;

namespace PaymentManagement.Tests
{
    public class CompanyServicesTests
    {
        private readonly Mock<ICompanyRepository> _mockCompanyRepository;
        private readonly CompanyServices _companyServices;

        public CompanyServicesTests()
        {
            _mockCompanyRepository = new Mock<ICompanyRepository>();
            _companyServices = new CompanyServices(_mockCompanyRepository.Object);
        }

        [Fact]
        public async Task GetCompanyById_ReturnsCompany_WhenCompanyExists()
        {
            var companyId = Guid.NewGuid();
            //var company = new Company { Id = companyId, Name = "Acme Corp", /*Email = "acme@test.com", Phone = "123", Address = "123 St", Country = "USA"*/ };
            var company = new Company("Acme Corp", "USA");
            company.Id = companyId;
            _mockCompanyRepository.Setup(x => x.GetCompanyById(companyId)).ReturnsAsync(company);

            var result = await _companyServices.GetCompanyById(companyId);

            Assert.NotNull(result);
            Assert.Equal("Acme Corp", result.Name);
        }

        [Fact]
        public async Task GetCompanyById_ThrowsException_WhenCompanyNotFound()
        {
            var companyId = Guid.NewGuid();
            _mockCompanyRepository.Setup(x => x.GetCompanyById(companyId)).ReturnsAsync((Company?)null);

            await Assert.ThrowsAsync<BusinessException>(() => _companyServices.GetCompanyById(companyId));
        }

        [Fact]
        public async Task GetCompanyById_ThrowsException_WhenIdIsEmpty()
        {
            await Assert.ThrowsAsync<BusinessException>(() => _companyServices.GetCompanyById(Guid.Empty));
        }

        [Fact]
        public async Task AddCompany_SetsId()
        {
            //var company = new Company { Name = "Acme Corp", Email = "acme@test.com", Phone = "123", Address = "123 St", Country = "USA" };
            var company = new Company("Acme Corp", "USA");
            _mockCompanyRepository.Setup(x => x.AddCompany(It.IsAny<Company>())).Returns(Task.CompletedTask);

            await _companyServices.AddCompany(company);

            Assert.NotEqual(Guid.Empty, company.Id);
            _mockCompanyRepository.Verify(x => x.AddCompany(It.IsAny<Company>()), Times.Once);
        }

        [Fact]
        public async Task AddCompany_ThrowsException_WhenCompanyIsNull()
        {
            await Assert.ThrowsAsync<BusinessException>(() => _companyServices.AddCompany(null!));
        }

        [Fact]
        public async Task DeleteCompany_CallsRepository()
        {
            var companyId = Guid.NewGuid();
            _mockCompanyRepository.Setup(x => x.DeleteCompany(companyId)).Returns(Task.CompletedTask);

            await _companyServices.DeleteCompany(companyId);

            _mockCompanyRepository.Verify(x => x.DeleteCompany(companyId), Times.Once);
        }

        [Fact]
        public async Task DeleteCompany_ThrowsException_WhenIdIsEmpty()
        {
            await Assert.ThrowsAsync<BusinessException>(() => _companyServices.DeleteCompany(Guid.Empty));
        }

        [Fact]
        public async Task UpdateCompany_CallsRepository()
        {
            //var company = new Company { Id = Guid.NewGuid(), Name = "Acme Corp", Email = "acme@test.com", Phone = "123", Address = "123 St", Country = "USA" };
            var company = new Company("Acme Corp", "USA");
            _mockCompanyRepository.Setup(x => x.UpdateCompany(It.IsAny<Company>())).Returns(Task.CompletedTask);

            await _companyServices.UpdateCompany(company);

            _mockCompanyRepository.Verify(x => x.UpdateCompany(It.IsAny<Company>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCompany_ThrowsException_WhenCompanyIsNull()
        {
            await Assert.ThrowsAsync<BusinessException>(() => _companyServices.UpdateCompany(null!));
        }
    }
}
