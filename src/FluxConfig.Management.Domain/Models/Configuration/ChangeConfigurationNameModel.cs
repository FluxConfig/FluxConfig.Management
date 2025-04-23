namespace FluxConfig.Management.Domain.Models.Configuration;

public record ChangeConfigurationNameModel(
    long ConfigurationId,
    string NewName
);