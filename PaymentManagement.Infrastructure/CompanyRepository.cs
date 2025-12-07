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

        public async Task<bool> AddCompany(Company company)
        {
            await _context.Company.AddAsync(company);
            int res = await _context.SaveChangesAsync();

            return res > 0;
        }

        public async Task<bool> DeleteCompany(Guid id)
        {
            var company = await _context.Company.FindAsync(id);
            if (company == null) return false;
            
            _context.Company.Remove(company);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCompany(Company company)
        {
            var c = await _context.Company.FindAsync(company.Id);
            if (c == null) return false;

            c.Name = company.Name;
            c.CountryCode = company.CountryCode;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
