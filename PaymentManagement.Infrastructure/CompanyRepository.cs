using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentManagement.Domain;

namespace PaymentManagement.Infrastructure
{
    public class CompanyRepository
    {
        private readonly AppDbContext _context;

        public CompanyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Company>> GetAllCompanies() => await _context.Company.ToListAsync();

        public async Task<Company> GetCompanyById(Guid id) => await _context.Company.FindAsync(id);

        public async Task AddCompany(Company company)
        {
            await _context.Company.AddAsync(company);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCompany(Guid id)
        {
            var company = await _context.Company.FindAsync(id);
            if (company == null) throw new BusinessException("Can't find the company");
            
            _context.Company.Remove(company);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCompany(Company company)
        {
            var com = await _context.Company.FindAsync(company.Id);
            if (com == null)
                throw new BusinessException("Can't find the company to update");

            _context.Company.Update(company);
            await _context.SaveChangesAsync();
        }
    }
}
