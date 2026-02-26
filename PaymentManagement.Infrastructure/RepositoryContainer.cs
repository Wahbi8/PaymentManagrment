using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace PaymentManagement.Infrastructure
{
    public static class RepositoryContainer
    {
        public static IServiceCollection AddRepository(this IServiceCollection Repository)
        {
            Repository.AddScoped<UserRepository>();
            Repository.AddScoped<InvoiceRepository>();
            Repository.AddScoped<AuthRepository>();
            Repository.AddScoped<CompanyRepository>();
            Repository.AddScoped<CustomerRepository>();
            Repository.AddScoped<PaymentRepository>();
            Repository.AddScoped<PaymentMethodRepository>();

            return Repository;
        }
    }
}
