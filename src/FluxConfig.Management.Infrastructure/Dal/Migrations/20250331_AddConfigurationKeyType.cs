using FluentMigrator;

namespace FluxConfig.Management.Infrastructure.Dal.Migrations;

[Migration(version:20250331, TransactionBehavior.Default)]
public class AddConfigurationKeyType: Migration
{
    public override void Up()
    {
        const string sql = @"
DO $$ 
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'configuration_key_type') THEN
            CREATE TYPE configuration_key_type as
            (
                id                      varchar(255),
                configuration_id        bigint,
                role_permission         user_config_role_enum,
                expiration_date         timestamp with time zone
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
        DROP TYPE IF EXISTS configuration_key_type;
    END
$$;
";
        
        Execute.Sql(sql);
    }
}