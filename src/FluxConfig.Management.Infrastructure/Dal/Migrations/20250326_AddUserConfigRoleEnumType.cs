using FluentMigrator;

namespace FluxConfig.Management.Infrastructure.Dal.Migrations;

[Migration(version:20250326, TransactionBehavior.Default)]
public class AddUserConfigRoleEnumType: Migration
{
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'user_config_role_enum') THEN
            CREATE TYPE user_config_role_enum AS ENUM 
            (
             'member',
             'admin'
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
        DROP TYPE IF EXISTS user_config_role_enum;
    END
$$;
";
        Execute.Sql(sql);
    }
}