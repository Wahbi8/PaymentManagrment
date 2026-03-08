using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PaymentManagement.Domain.Interfaces;

namespace PaymentManagement.Infrastructure
{
    public static class RepositoryContainer
    {
        public static IServiceCollection AddRepository(this IServiceCollection Repository)
        {
            Repository.AddScoped<IUserRepository, UserRepository>();
            Repository.AddScoped<IInvoiceRepository, InvoiceRepository>();
            Repository.AddScoped<IAuthRepository, AuthRepository>();
            Repository.AddScoped<ICompanyRepository, CompanyRepository>();
            Repository.AddScoped<ICustomerRepository, CustomerRepository>();
            Repository.AddScoped<IPaymentRepository, PaymentRepository>();
            Repository.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
            Repository.AddScoped<IInvoiceLineItemRepository, InvoiceLineItemRepository>();
            Repository.AddScoped<IAuditTrailRepository, AuditTrailRepository>();

            return Repository;
        }
    }
}
