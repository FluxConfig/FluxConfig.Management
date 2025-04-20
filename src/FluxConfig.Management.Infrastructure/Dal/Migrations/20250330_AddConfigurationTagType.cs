using FluentMigrator;

namespace FluxConfig.Management.Infrastructure.Dal.Migrations;

[Migration(version:20250330, TransactionBehavior.Default)]
public class AddConfigurationTagType: Migration 
{
    public override void Up()
    {
        const string sql = @"
DO $$ 
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'configuration_tag_type') THEN
            CREATE TYPE configuration_tag_type as
            (
                id                      bigint,
                configuration_id        bigint,
                tag                     varchar(30),
                description             varchar(500),
                required_role           user_config_role_enum
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
        DROP TYPE IF EXISTS configuration_tag_type;
    END
$$;
";
        
        Execute.Sql(sql);
    }
}