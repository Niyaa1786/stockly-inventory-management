using FluentValidation;

namespace Stockly.Application.Features.Warehouses.GetWarehouses;

public class GetWarehousesValidator : AbstractValidator<GetWarehousesRequest>
{
    public GetWarehousesValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Page must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

        RuleFor(x => x.Keyword)
            .MaximumLength(100).WithMessage("Keyword must not exceed 100 characters.");
    }
}
