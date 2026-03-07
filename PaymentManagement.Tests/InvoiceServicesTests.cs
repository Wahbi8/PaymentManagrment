using Moq;
using PaymentManagement.Domain.Interfaces;
using PaymentManagement.Application.Services;
using PaymentManagement.Domain;
using Xunit;

namespace PaymentManagement.Tests
{
    public class InvoiceServicesTests
    {
        private readonly Mock<IInvoiceRepository> _mockInvoiceRepository;
        private readonly InvoiceServices _invoiceServices;

        public InvoiceServicesTests()
        {
            _mockInvoiceRepository = new Mock<IInvoiceRepository>();
            _invoiceServices = new InvoiceServices(_mockInvoiceRepository.Object);
        }

        [Fact]
        public async Task GetInvoiceById_ReturnsInvoice_WhenInvoiceExists()
        {
            var invoiceId = Guid.NewGuid();
            var invoice = new Invoice
            {
                Id = invoiceId,
                CompanyId = Guid.NewGuid(),
                TotalAmount = 1000,
                Status = InvoiceStatus.draft,
                IssueDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(30)
            };
            _mockInvoiceRepository.Setup(x => x.GetInvoiceById(invoiceId)).ReturnsAsync(invoice);

            var result = await _invoiceServices.GetInvoiceById(invoiceId);

            Assert.NotNull(result);
            Assert.Equal(1000, result.TotalAmount);
        }

        [Fact]
        public async Task GetInvoiceById_ThrowsException_WhenInvoiceNotFound()
        {
            var invoiceId = Guid.NewGuid();
            _mockInvoiceRepository.Setup(x => x.GetInvoiceById(invoiceId)).ReturnsAsync((Invoice?)null);

            await Assert.ThrowsAsync<BusinessException>(() => _invoiceServices.GetInvoiceById(invoiceId));
        }

        [Fact]
        public async Task GetInvoiceById_ThrowsException_WhenIdIsEmpty()
        {
            await Assert.ThrowsAsync<BusinessException>(() => _invoiceServices.GetInvoiceById(Guid.Empty));
        }

        [Fact]
        public async Task AddInvoice_SetsId()
        {
            var invoice = new Invoice
            {
                CompanyId = Guid.NewGuid(),
                TotalAmount = 1000,
                Status = InvoiceStatus.draft,
                IssueDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(30)
            };
            _mockInvoiceRepository.Setup(x => x.AddInvoice(It.IsAny<Invoice>())).Returns(Task.CompletedTask);

            await _invoiceServices.AddInvoice(invoice);

            Assert.NotEqual(Guid.Empty, invoice.Id);
            _mockInvoiceRepository.Verify(x => x.AddInvoice(It.IsAny<Invoice>()), Times.Once);
        }

        [Fact]
        public async Task AddInvoice_ThrowsException_WhenInvoiceIsNull()
        {
            await Assert.ThrowsAsync<BusinessException>(() => _invoiceServices.AddInvoice(null!));
        }

        [Fact]
        public async Task UpdateInvoice_ThrowsException_WhenStatusIsNotDraft()
        {
            var invoice = new Invoice
            {
                Id = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                TotalAmount = 1000,
                Status = InvoiceStatus.paid,
                IssueDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(30)
            };

            await Assert.ThrowsAsync<BusinessException>(() => _invoiceServices.UpdateInvoice(invoice));
        }

        [Fact]
        public async Task UpdateInvoice_CallsRepository_WhenStatusIsDraft()
        {
            var invoice = new Invoice
            {
                Id = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                TotalAmount = 1000,
                Status = InvoiceStatus.draft,
                IssueDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(30)
            };
            _mockInvoiceRepository.Setup(x => x.UpdateInvoice(It.IsAny<Invoice>())).Returns(Task.CompletedTask);

            await _invoiceServices.UpdateInvoice(invoice);

            _mockInvoiceRepository.Verify(x => x.UpdateInvoice(It.IsAny<Invoice>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInvoice_ThrowsException_WhenStatusIsNotDraft()
        {
            var invoiceId = Guid.NewGuid();
            var invoice = new Invoice
            {
                Id = invoiceId,
                CompanyId = Guid.NewGuid(),
                TotalAmount = 1000,
                Status = InvoiceStatus.sent,
                IssueDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(30)
            };
            _mockInvoiceRepository.Setup(x => x.GetInvoiceById(invoiceId)).ReturnsAsync(invoice);

            await Assert.ThrowsAsync<BusinessException>(() => _invoiceServices.DeleteInvoice(invoiceId));
        }

        [Fact]
        public async Task GetInvoicesByCompanyId_ReturnsList_WhenInvoicesExist()
        {
            var companyId = Guid.NewGuid();
            var invoices = new List<Invoice>
            {
                new Invoice { Id = Guid.NewGuid(), CompanyId = companyId, TotalAmount = 1000, Status = InvoiceStatus.draft, IssueDate = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(30) },
                new Invoice { Id = Guid.NewGuid(), CompanyId = companyId, TotalAmount = 2000, Status = InvoiceStatus.sent, IssueDate = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(30) }
            };
            _mockInvoiceRepository.Setup(x => x.GetInvoicesByCompanyId(companyId)).ReturnsAsync(invoices);

            var result = await _invoiceServices.GetInvoicesByCompanyId(companyId);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetInvoicesByCompanyId_ThrowsException_WhenIdIsEmpty()
        {
            await Assert.ThrowsAsync<BusinessException>(() => _invoiceServices.GetInvoicesByCompanyId(Guid.Empty));
        }

        [Fact]
        public async Task GetAllInvoices_ReturnsList()
        {
            var invoices = new List<Invoice>
            {
                new Invoice { Id = Guid.NewGuid(), CompanyId = Guid.NewGuid(), TotalAmount = 1000, Status = InvoiceStatus.draft, IssueDate = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(30) }
            };
            _mockInvoiceRepository.Setup(x => x.GetAllInvoices()).ReturnsAsync(invoices);

            var result = await _invoiceServices.GetAllInvoices();

            Assert.Single(result);
        }
    }
}
