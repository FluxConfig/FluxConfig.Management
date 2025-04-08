using FluentMigrator;

namespace FluxConfig.Management.Infrastructure.Dal.Migrations;

[Migration(version:20250323, TransactionBehavior.Default)]
public class AddUserCredentialsType: Migration
{
    public override void Up()
    {
        const string sql = @"
DO $$ 
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'user_credentials_type') THEN
            CREATE TYPE user_credentials_type as
            (
                id           bigint,
                username     varchar(100),
                email        varchar(100),
                password     varchar(255)
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
        DROP TYPE IF EXISTS user_credentials_type;
    END
$$;
";
        Execute.Sql(sql);
    }
}