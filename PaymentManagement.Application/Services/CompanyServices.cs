using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentManagement.Domain;
using PaymentManagement.Infrastructure;

namespace PaymentManagement.Application.Services
{
    public class CompanyServices
    {
        private readonly CompanyRepository _companyRepository;

        public CompanyServices(CompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<Company> GetCompanyById(Guid id)
        {
            if (id == Guid.Empty)
                throw new BusinessException("Company id cannot be empty");
            var company = await _companyRepository.GetCompanyById(id);
            if (company == null)
                throw new BusinessException("Company not found");
            return company;
        }

        public async Task AddCompany(Company company)
        {
            if (company == null)
                throw new BusinessException("Company fields are empty");

            company.Id = Guid.NewGuid();
            await _companyRepository.AddCompany(company);
        }

        public async Task DeleteCompany(Guid id)
        {
            if (id == Guid.Empty) 
                throw new BusinessException("Then id is empty");

            await _companyRepository.DeleteCompany(id);
        } 

        public async Task UpdateCompany(Company company)
        {
            if (company == null)
                throw new BusinessException($"{nameof(Company)} cannot be null");

            await _companyRepository.UpdateCompany(company);
        }

    }
}
