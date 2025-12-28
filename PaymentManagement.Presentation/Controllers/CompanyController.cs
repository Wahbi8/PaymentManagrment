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

        [HttpGet]
        public async Task<Company> GetCompanyByUserId()
        {
            return await _companyService.GetCompanyByUserId(User.GetUserId());
        }
    }
}
