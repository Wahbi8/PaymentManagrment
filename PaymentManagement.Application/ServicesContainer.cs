using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PaymentManagement.Application.Services;
using PaymentManagement.Application.Validators;
using PaymentManagement.Infrastructure;

namespace PaymentManagement.Application
{
    public static class ServicesContainer
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            

            services.AddScoped<UserServices>();
            services.AddScoped<InvoiceServices>();
            services.AddScoped<AuthServices>();
            services.AddScoped<CompanyServices>();
            services.AddScoped<CustomerServices>();
            services.AddScoped<PaymentServices>();
            services.AddScoped<PaymentMethodServices>();
            
            return services;
        }
    }
}
