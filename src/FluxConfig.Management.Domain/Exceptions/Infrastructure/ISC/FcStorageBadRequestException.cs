namespace FluxConfig.Management.Domain.Exceptions.Infrastructure.ISC;

public class FcStorageBadRequestException: InfrastructureException
{
    public Dictionary<string, string> FiledViolations { get; }
    
    public FcStorageBadRequestException(string? message, Dictionary<string, string> filedViolations, Exception? innerException) : base(message, innerException)
    {
        FiledViolations = filedViolations;
    }
}