using FluentMigrator;

namespace FluxConfig.Management.Infrastructure.Dal.Migrations;

[Migration(version:20250324, TransactionBehavior.Default)]
public class AddUserSessionType: Migration
{
    public override void Up()
    {
        const string sql = @"
DO $$ 
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'user_session_type') THEN
            CREATE TYPE user_session_type as
            (
                id                  varchar(255),
                user_id             bigint,
                expiration_date     timestamp with time zone
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
        DROP TYPE IF EXISTS user_session_type;
    END
$$;
";
        Execute.Sql(sql);
    }
}