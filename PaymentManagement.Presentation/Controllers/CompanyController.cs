using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PaymentManagement.Domain;
using PaymentManagement.Application.Services;
using PaymentManagement.Presentation.Common.Extensions;

namespace PaymentManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyServices _companyService;
        public CompanyController(CompanyServices companyService)
        {
            _companyService = companyService;
        }

        [HttpGet("GetUserCompanies")]
        public async Task<Company> GetCompanyByUserId()
        {
            return await _companyService.GetCompanyByUserId(User.GetUserId());
        }

        [HttpGet("GetCompanyById")]
        public async Task<Company> GetCompanyById([FromBody] Guid id)
        {
            return await _companyService.GetCompanyById(id);
        }

        [HttpPost("AddCompany")]
        public async Task<IActionResult> AddCompany([FromBody] Company company)
        {
            await _companyService.AddCompany(company);
            return Ok();
        }

        //[HttpDelete("")]
        //public async Task<IActionResult> DeleteCompany(Guid id)
        //{
        //    await _companyService.DeleteCompany(id);
        //    return Ok();
        //}

        [HttpPut("UodateCompany")]
        public async Task<IActionResult> UpdateCompany([FromBody] Company company)
        {
            await _companyService.UpdateCompany(company);
            return Ok();
        }
    }
}
