using FluentMigrator;

namespace FluxConfig.Management.Infrastructure.Dal.Migrations;

[Migration(version:20250401, TransactionBehavior.Default)]
public class AddConfigurationUsersView: Migration {
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
            CREATE VIEW configuration_users_view AS
                SELECT
                    conf.user_id,
                    conf.configuration_id,
                    conf.role,
                    cred.username,
                    cred.email
                FROM user_configurations AS conf
                INNER JOIN user_credentials AS cred ON conf.user_id = cred.id;
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
        DROP VIEW IF EXISTS configuration_users_view;
    END
$$;
";
        
        Execute.Sql(sql);
    }
}