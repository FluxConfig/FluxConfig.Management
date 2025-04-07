using FluentValidation;

namespace FluxConfig.Management.Domain.Exceptions.Domain;

public class BadRequestException: DomainException
{
    public BadRequestException(string? message, ValidationException? innerException) : base(message, innerException)
    {
    }
}