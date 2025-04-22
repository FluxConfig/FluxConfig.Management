namespace FluxConfig.Management.Domain.Exceptions.Domain.User;

public class AdminChangeHisRoleException: DomainException
{
    public long AdminId { get; }
    public AdminChangeHisRoleException(string? message, long adminId) : base(message)
    {
        AdminId = adminId;
    }
}