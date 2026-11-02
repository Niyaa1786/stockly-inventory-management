using FluentValidation.Results;

namespace Stockly.Application.Common.Exceptions;

public class AppValidationException : FluentValidation.ValidationException
{
    public AppValidationException(string propertyName, string errorMessage) : base(new[] { new ValidationFailure(propertyName, errorMessage) }) { }
}
