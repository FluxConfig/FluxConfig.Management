using FluentMigrator;

namespace FluxConfig.Management.Infrastructure.Dal.Migrations;

[Migration(version:20250321, TransactionBehavior.Default)]
public class AddUserGlobalRoleEType: Migration 
{
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'user_global_role_enum') THEN
            CREATE TYPE user_global_role_enum AS ENUM 
            (
             'member',
             'trusted',
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
        DROP TYPE IF EXISTS user_global_role_enum;
    END
$$;
";
        
        Execute.Sql(sql);
    }
}