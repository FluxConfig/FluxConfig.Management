using FluentMigrator;

namespace FluxConfig.Management.Infrastructure.Dal.Migrations;

[Migration(version:20250406, TransactionBehavior.Default)]
public class AddUserConfigurationsViewType: Migration 
{
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'user_configurations_view_type') THEN
            CREATE TYPE user_configurations_view_type as
            (
                user_id              bigint,
                configuration_id     bigint,
                role                 user_config_role_enum,
                name                 varchar(100),
                description          varchar(500)
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
        DROP TYPE IF EXISTS user_configurations_view_type;
    END
$$;
";
        
        Execute.Sql(sql);
    }
}