using FluentMigrator;

namespace FluxConfig.Management.Infrastructure.Dal.Migrations;

[Migration(version:20250325, TransactionBehavior.Default)]
public class AddUserGlobalRoleType: Migration
{
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'user_global_role_type') THEN
            CREATE TYPE user_global_role_type as
            (
                id          bigint,
                user_id     bigint,
                role        user_global_role_enum
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
        DROP TYPE IF EXISTS user_global_role_type;
    END
$$;
";
        Execute.Sql(sql);
    }
}