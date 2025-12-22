using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentManagement.Domain;
using PaymentManagement.Infrastructure;

namespace PaymentManagement.Application.Services
{
    public class PaymentServices
    {
        private readonly PaymentRepository _PaymentRepository;
        private readonly PaymentMethodRepository _paymentMethodRepository;

        public PaymentServices(PaymentRepository paymentRepository, PaymentMethodRepository paymentMethodRepository)
        {
            _PaymentRepository = paymentRepository;
            _paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<List<Payment>> GetAllPayments()
        {
            var payments = await _PaymentRepository.GetAllPayments();
            return payments;
        }

        public async Task<List<Payment>> GetPaymentsByInvoiceId(Guid id)
        {
            var payments = await _PaymentRepository.GetPaymentByInvoiceId(id);
            return payments;
        }

        public async Task AddPayment(Payment payment) 
        {
            if (payment == null)
                throw new BusinessException("Payment fields are required");

            bool hasExistingMethod = payment.PaymentMethodId != null;
            bool hasNewMethod = payment.PaymentMethod != null;

            if (hasExistingMethod == hasNewMethod)
                throw new BusinessException("Provide either an existing payment method or a new one");

            if (hasNewMethod)
            {
                await _paymentMethodRepository.AddPaymentMethod(payment.PaymentMethod!);
                payment.PaymentMethodId = payment.PaymentMethod!.Id;
            }

            await _PaymentRepository.AddPayment(payment);

        }

    }
}
