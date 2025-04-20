using FluentMigrator;

namespace FluxConfig.Management.Infrastructure.Dal.Migrations;

[Migration(version:20250329, TransactionBehavior.Default)]
public class AddUserConfigurationsType: Migration
{
    public override void Up()
    {
        const string sql = @"
DO $$ 
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'user_configurations_type') THEN
            CREATE TYPE user_configurations_type as
            (
                user_id                 bigint,
                role                    user_config_role_enum,
                configuration_id        bigint
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
        DROP TYPE IF EXISTS user_configurations_type;
    END
$$;
";
        
        Execute.Sql(sql);
    }
}