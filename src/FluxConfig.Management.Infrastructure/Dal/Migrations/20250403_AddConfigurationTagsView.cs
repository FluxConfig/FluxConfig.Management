using FluentMigrator;

namespace FluxConfig.Management.Infrastructure.Dal.Migrations;

[Migration(version: 20250403, TransactionBehavior.Default)]
public class AddConfigurationTagsView: Migration
{
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
            CREATE VIEW configuration_tags_view AS
                SELECT
                    conf.id as configuration_id,
                    conf.name,
                    conf.storage_key,
                    conf.description as configuration_description,
                    tags.id as tag_id,
                    tags.tag as tag_name,
                    tags.required_role,
                    tags.description as tag_description
                FROM configuration_tags AS tags
                INNER JOIN configurations AS conf ON tags.configuration_id = conf.id;
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
        DROP VIEW IF EXISTS configuration_tags_view;
    END
$$;
";
        
        Execute.Sql(sql);
    }
}