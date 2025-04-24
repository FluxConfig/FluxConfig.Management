using FluentMigrator;

namespace FluxConfig.Management.Infrastructure.Dal.Migrations;

[Migration(version:20250404, TransactionBehavior.Default)]
public class AddConfigurationTagsViewType: Migration
{
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'configuration_tags_view_type') THEN
            CREATE TYPE configuration_tags_view_type as
            (
                configuration_id             bigint,
                name                         varchar(100),
                storage_key                  varchar(255),
                configuration_description    varchar(500),
                tag_id                       bigint,
                tag_name                     varchar(30),
                required_role                user_config_role_enum,
                tag_description              varchar(500)
            );
        END IF;
    END
$$;
";
        
        Execute.Sql(sql);
    }

    public override void Down()
    {
        const string sql = @"
DO $$
    BEGIN
        DROP TYPE IF EXISTS configuration_tags_view_type;
    END
$$;
";
        
        Execute.Sql(sql);
    }
}