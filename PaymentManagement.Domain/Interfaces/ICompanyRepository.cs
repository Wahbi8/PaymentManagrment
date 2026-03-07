using PaymentManagement.Domain;

namespace PaymentManagement.Domain.Interfaces
{
    public interface ICompanyRepository
    {
        Task<List<Company>> GetAllCompanies();
        Task<Company> GetCompanyById(Guid id);
        Task<Company> GetCompanyByUserId(Guid id);
        Task AddCompany(Company company);
        Task DeleteCompany(Guid id);
        Task UpdateCompany(Company company);
    }
}
