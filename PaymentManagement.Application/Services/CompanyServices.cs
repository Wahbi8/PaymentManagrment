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
            var company = await _companyRepository.GetCompanyById(id);
            return company;
        }

        public async Task AddCompany(Company company)
        {
            if (company == null)
                throw new BusinessException("Company fields are empty");


        }
    }
}
