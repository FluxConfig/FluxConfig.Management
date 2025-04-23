using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Services.Interfaces;

public interface IConfigurationTagsService
{
    public Task ChangeTagDescription(long tagId, string newDescription, CancellationToken cancellationToken);
    public Task ChangeTagRequiredRole(long tagId, UserConfigRole newRole, CancellationToken cancellationToken);
    public Task<IReadOnlyList<ConfigurationTagModel>> GetConfigurationTags(long configurationId, UserConfigRole userRole,
        CancellationToken cancellationToken);
    public Task<ConfigurationTagsViewModel> GetTagMeta(long tagId, long userId, UserGlobalRole userRole, CancellationToken cancellationToken);
    public Task CreateTag(ConfigurationTagModel model, CancellationToken cancellationToken);
    public Task DeleteTag(long tagId, CancellationToken cancellationToken);
}