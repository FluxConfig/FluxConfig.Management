using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Models.Auth;

namespace FluxConfig.Management.Domain.Mappers.Auth;

internal static class EntityMapper
{
    internal static SessionModel MapEntityToModel(this UserSessionEntity entity)
    {
        return new SessionModel(
            Id: entity.Id,
            UserId: entity.UserId,
            ExpirationDate: entity.ExpirationDate
        );
    }
}