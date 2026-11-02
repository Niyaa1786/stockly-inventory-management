using FluentValidation;

namespace Stockly.Application.Features.Users.UpdateUser;

public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
            .EmailAddress().WithMessage("Email is invalid.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(100).WithMessage("Full name must not exceed 100 characters.");

        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("Role is invalid.");
    }
}
