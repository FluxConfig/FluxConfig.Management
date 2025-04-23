using FluentMigrator;

namespace FluxConfig.Management.Infrastructure.Dal.Migrations;

[Migration(version: 20250405, TransactionBehavior.Default)]
public class AddUserConfigurationsView: Migration
{
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
            CREATE VIEW user_configurations_view AS
                SELECT
                    uconf.user_id,
                    uconf.configuration_id,
                    uconf.role,
                    conf.name,
                    conf.description
                FROM user_configurations AS uconf
                INNER JOIN configurations AS conf ON uconf.configuration_id = conf.id;
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
        DROP VIEW IF EXISTS user_configurations_view;
    END
$$;
";
        
        Execute.Sql(sql);
    }
}