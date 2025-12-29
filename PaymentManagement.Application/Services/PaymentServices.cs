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
        private readonly InvoiceRepository _invoiceRepository;

        public PaymentServices(PaymentRepository paymentRepository,
            PaymentMethodRepository paymentMethodRepository,
            InvoiceRepository invoiceRepository)
        {
            _PaymentRepository = paymentRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _invoiceRepository = invoiceRepository;
        }

        public async Task<List<Payment>> GetAllPayments()
        {
            var payments = await _PaymentRepository.GetAllPayments();
            if (payments == null || !payments.Any())
                throw new BusinessException("No payments found");
            return payments;
        }

        public async Task<List<Payment>> GetPaymentsByInvoiceId(Guid id)
        {
            if (id == Guid.Empty)
                throw new BusinessException("InvoiceId cannot be empty");

            var payments = await _PaymentRepository.GetPaymentByInvoiceId(id);
            if (payments == null || !payments.Any())
                throw new BusinessException("No payments found for the invoice");
            return payments;
        }
        
        public async Task<List<Payment>> GetAllPaymentsByUserId(Guid id)
        {
            if (id == Guid.Empty)
                throw new BusinessException("UserId cannot be empty");

            var payments = await _PaymentRepository.GetAllPaymentsByUserId(id);
            if (payments == null || !payments.Any())
                throw new BusinessException("No payments found for the invoice");
            return payments;
        }

        public async Task AddPayment(Payment payment)
        {
            if (payment == null)
                throw new BusinessException("Payment fields are required");

            if (payment.InvoiceId == Guid.Empty)
                throw new BusinessException("Payment need Invoice to be processed");

            bool hasExistingMethod = payment.PaymentMethodId != null;
            bool hasNewMethod = payment.PaymentMethod != null;

            if (hasExistingMethod == hasNewMethod)
                throw new BusinessException("Provide either an existing payment method or a new one");

            if (hasNewMethod)
            {
                await _paymentMethodRepository.AddPaymentMethod(payment.PaymentMethod!);
                payment.PaymentMethodId = payment.PaymentMethod!.Id;
            }

            payment.Invoice = await _invoiceRepository.GetInvoiceById(payment.InvoiceId);
            if (payment.Invoice == null)
                throw new BusinessException("Can not find the the invoice");

            payment.Invoice.ApplyPayment(payment.Amount);
            payment.Id = Guid.NewGuid();
            await _PaymentRepository.AddPayment(payment);
        }

    }
}
