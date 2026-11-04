using FluentValidation;

namespace Stockly.Application.Features.Suppliers.CreateSupplier;

public class CreateSupplierValidator : AbstractValidator<CreateSupplierRequest>
{
    public CreateSupplierValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("Phone must not exceed 20 characters.");

        RuleFor(x => x.Email)
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
            .EmailAddress().WithMessage("Email is invalid.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("Address must not exceed 500 characters.");
    }
}
