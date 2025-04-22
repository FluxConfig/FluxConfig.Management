using System.Data;
using FluentMigrator;
using FluentMigrator.Postgres;

namespace FluxConfig.Management.Infrastructure.Dal.Migrations;

[Migration(version:20250327, TransactionBehavior.Default)]
public class AddConfigurationsScheme: Migration
{
    public override void Up()
    {
        // Configurations
        Create.Table("configurations")
            .WithColumn("id").AsInt64().PrimaryKey("configurations_pk").Identity()
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("storage_key").AsString(255).NotNullable()
            .WithColumn("description").AsString(500).NotNullable();

        // User configurations
        Create.Table("user_configurations")
            .WithColumn("user_id").AsInt64().NotNullable()
            .WithColumn("role").AsCustom("user_config_role_enum").NotNullable()
            .WithColumn("configuration_id").AsInt64().NotNullable();

        Create.ForeignKey("FK_user_configurations_uid")
            .FromTable("user_configurations").ForeignColumn("user_id")
            .ToTable("user_credentials").PrimaryColumn("id")
            .OnDelete(Rule.Cascade);

        Create.ForeignKey("FK_user_configurations_conf_id")
            .FromTable("user_configurations").ForeignColumn("configuration_id")
            .ToTable("configurations").PrimaryColumn("id")
            .OnDelete(Rule.Cascade);

        Create.PrimaryKey("user_configurations_pk")
            .OnTable("user_configurations")
            .Columns("user_id", "configuration_id");
        
        // Configuration keys
        Create.Table("configuration_keys")
            .WithColumn("id").AsString(255).PrimaryKey("configuration_keys_pk").Unique()
            .WithColumn("configuration_id").AsInt64().NotNullable()
            .WithColumn("role_permission").AsCustom("user_config_role_enum").NotNullable()
            .WithColumn("expiration_date").AsDateTimeOffset().NotNullable();


        Create.ForeignKey("FK_configuration_keys_conf_id")
            .FromTable("configuration_keys").ForeignColumn("configuration_id")
            .ToTable("configurations").PrimaryColumn("id")
            .OnDelete(Rule.Cascade);

        // Configuration tags
        Create.Table("configuration_tags")
            .WithColumn("id").AsInt64().PrimaryKey("configuration_tags_pk").Identity()
            .WithColumn("configuration_id").AsInt64().NotNullable()
            .WithColumn("tag").AsString(30).NotNullable().Unique()
            .WithColumn("description").AsString(500).NotNullable()
            .WithColumn("required_role").AsCustom("user_config_role_enum").NotNullable();
        
        Create.ForeignKey("FK_configuration_tags_conf_id")
            .FromTable("configuration_tags").ForeignColumn("configuration_id")
            .ToTable("configurations").PrimaryColumn("id")
            .OnDelete(Rule.Cascade);

    }

    public override void Down()
    {
        Delete.Table("user_configurations");
        Delete.Table("configuration_tags");
        Delete.Table("configuration_keys");
        Delete.Table("configurations");
    }
}