using FluentMigrator;

namespace FluxConfig.Management.Infrastructure.Dal.Migrations;

[Migration(version: 20250328, TransactionBehavior.Default)]
public class AddConfigurationType: Migration
{
    public override void Up()
    {
        const string sql = @"
DO $$ 
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'configuration_type') THEN
            CREATE TYPE configuration_type as
            (
                id              bigint,
                name            varchar(100),
                storage_key     varchar(255),
                description     varchar(500)
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
        DROP TYPE IF EXISTS configuration_type;
    END
$$;
";
        
        Execute.Sql(sql);
    }
}