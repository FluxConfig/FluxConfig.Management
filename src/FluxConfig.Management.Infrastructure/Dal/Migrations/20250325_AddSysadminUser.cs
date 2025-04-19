using FluentMigrator;
using FluxConfig.Management.Domain.Hasher;
using FluxConfig.Management.Infrastructure.Configuration;
using FluxConfig.Management.Infrastructure.Configuration.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace FluxConfig.Management.Infrastructure.Dal.Migrations;

[Migration(version:20250325, TransactionBehavior.Default)]
public class AddSysadminUser: Migration
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _hostEnvironment;

    public AddSysadminUser(IConfiguration configuration, IHostEnvironment environment)
    {
        _configuration = configuration;
        _hostEnvironment = environment;
    }
    
    public override void Up()
    {
        FcmSysadminOptions adminInitOptions = ConfigurationGetter.GetSysadminInitOptions(
            configuration: _configuration,
            isDevelopment: _hostEnvironment.IsDevelopment());
        

        Insert.IntoTable("user_credentials")
            .Row(new
            {
                username = adminInitOptions.Username,
                email = adminInitOptions.Email,
                password = PasswordHasher.Hash(adminInitOptions.Password),
                role = "admin"
            });
    }

    public override void Down()
    {
        FcmSysadminOptions adminInitOptions = ConfigurationGetter.GetSysadminInitOptions(
            configuration: _configuration,
            isDevelopment: _hostEnvironment.IsDevelopment()
        );

        Delete.FromTable("user_credentials")
            .Row(new { email = adminInitOptions.Email });
    }
}