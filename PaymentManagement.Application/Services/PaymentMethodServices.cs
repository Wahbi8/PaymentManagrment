using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentManagement.Domain.Entities;
using PaymentManagement.Infrastructure;

namespace PaymentManagement.Application.Services
{
    public class PaymentMethodServices
    {
        private readonly PaymentMethodRepository _paymentMethodRepo;

        public PaymentMethodServices(PaymentMethodRepository paymentMethodRepo)
        {
            _paymentMethodRepo = paymentMethodRepo;
        }

        public async Task<List<PaymentMethod>> GetPaymentMethodsByUserId(Guid id)
        {
            var paymentMethods = await _paymentMethodRepo.GetPaymentMethodsByUserId(id);
            return paymentMethods;
        }

        public async Task<bool> AddPaymrntMethod(PaymentMethod paymentMethod)
        {
            return await _paymentMethodRepo.AddPaymentMethod(paymentMethod);
        }
    }
}
