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

        public PaymentServices(PaymentRepository paymentRepository)
        {
            _PaymentRepository = paymentRepository;
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

        public async Task<bool> AddPayment(Payment payment) 
        {
            // add paymentMethod to the payment before adding payment
        }

    }
}
