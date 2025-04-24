using FluxConfig.Management.Domain.Exceptions.Infrastructure;

namespace FluxConfig.Management.Domain.Exceptions.Domain.Config;

public class UserDoesntBelongToConfigException: DomainException
{
    public long UserId { get; }
    public long ConfigId { get; }
    
    public UserDoesntBelongToConfigException(string? message, long userId, long configId, EntityNotFoundException? innerException) : base(message, innerException)
    {
        UserId = userId;
        ConfigId = configId;
    }
}