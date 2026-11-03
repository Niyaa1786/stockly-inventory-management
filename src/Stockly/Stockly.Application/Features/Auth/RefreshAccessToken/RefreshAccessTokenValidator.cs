using FluentValidation;

namespace Stockly.Application.Features.Auth.RefreshAccessToken;

public class RefreshAccessTokenValidator : AbstractValidator<RefreshAccessTokenRequest>
{
    public RefreshAccessTokenValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.")
            .MaximumLength(200).WithMessage("Refresh token is invalid.");
    }
}
