using FluentMigrator;

namespace FluxConfig.Management.Infrastructure.Dal.Migrations;

[Migration(version:20250402 , TransactionBehavior.Default)]
public class AddConfigurationUsersViewType: Migration
{
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'configuration_user_view_type') THEN
            CREATE TYPE configuration_user_view_type as
            (
                user_id              bigint,
                configuration_id     bigint,
                role                 user_config_role_enum,
                username             varchar(100),
                email                varchar(100)
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
        DROP TYPE IF EXISTS configuration_user_view_type;
    END
$$;
";
        
        Execute.Sql(sql);
    }
}