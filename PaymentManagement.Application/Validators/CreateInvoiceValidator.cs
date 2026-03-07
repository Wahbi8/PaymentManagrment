using FluentValidation;
using PaymentManagement.Application.DTO;

namespace PaymentManagement.Application.Validators
{
    public class CreateInvoiceValidator : AbstractValidator<CreateInvoiceDto>
    {
        public CreateInvoiceValidator()
        {
            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("Company ID is required");

            RuleFor(x => x.IssueDate)
                .NotEmpty().WithMessage("Issue date is required");

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("Due date is required")
                .GreaterThan(x => x.IssueDate).WithMessage("Due date must be after issue date");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0).WithMessage("Total amount must be greater than 0");
        }
    }
}
