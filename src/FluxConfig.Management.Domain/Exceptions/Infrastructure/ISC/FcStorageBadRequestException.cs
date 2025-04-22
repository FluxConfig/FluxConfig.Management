namespace FluxConfig.Management.Domain.Exceptions.Infrastructure.ISC;

// TODO: Преобразовывать в локальный тип если надо
public class FcStorageBadRequestException: InfrastructureException
{
    public Dictionary<string, string> FiledViolations { get; }
    
    public FcStorageBadRequestException(string? message, Dictionary<string, string> filedViolations, Exception? innerException) : base(message, innerException)
    {
        FiledViolations = filedViolations;
    }
}