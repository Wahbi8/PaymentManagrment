using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PaymentManagement.Infrastructure;

namespace PaymentManagement.Application
{
    //TODO: continue this 
    public static class ServicesContainer
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // 1. Register Repositories
            services.AddScoped<UserRepository>();
            services.AddScoped<InvoiceRepository>();
            // Add new repositories here...

            return services;
        }
    }
}
