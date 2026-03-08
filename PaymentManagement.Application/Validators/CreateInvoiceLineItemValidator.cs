using FluentValidation;
using PaymentManagement.Application.DTO;

namespace PaymentManagement.Application.Validators
{
    public class CreateInvoiceLineItemValidator : AbstractValidator<CreateInvoiceLineItemDto>
    {
        public CreateInvoiceLineItemValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than 0");

            RuleFor(x => x.TaxRate)
                .InclusiveBetween(0, 100).WithMessage("Tax rate must be between 0 and 100");
        }
    }
    
    public class CreateInvoiceWithLineItemsValidator : AbstractValidator<CreateInvoiceWithLineItemsDto>
    {
        public CreateInvoiceWithLineItemsValidator()
        {
            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("Company ID is required");

            RuleFor(x => x.IssueDate)
                .NotEmpty().WithMessage("Issue date is required");

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("Due date is required")
                .GreaterThan(x => x.IssueDate).WithMessage("Due date must be after issue date");

            RuleFor(x => x.LineItems)
                .NotEmpty().WithMessage("At least one line item is required");

            RuleForEach(x => x.LineItems)
                .SetValidator(new CreateInvoiceLineItemValidator());
        }
    }
}
