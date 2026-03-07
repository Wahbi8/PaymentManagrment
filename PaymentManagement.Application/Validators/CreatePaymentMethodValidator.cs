using FluentValidation;
using PaymentManagement.Application.DTO;

namespace PaymentManagement.Application.Validators
{
    public class CreatePaymentMethodValidator : AbstractValidator<CreatePaymentMethodDto>
    {
        public CreatePaymentMethodValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required");

            RuleFor(x => x.Type)
                .InclusiveBetween(0, 5).WithMessage("Invalid payment method type");

            RuleFor(x => x.PaymentToken)
                .NotEmpty().WithMessage("Payment token is required");

            RuleFor(x => x.CardBrand)
                .MaximumLength(50).WithMessage("Card brand cannot exceed 50 characters");

            RuleFor(x => x.BankName)
                .MaximumLength(100).WithMessage("Bank name cannot exceed 100 characters");
        }
    }
}
