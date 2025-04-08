using System.Data;
using FluentMigrator;
using FluentMigrator.Postgres;

namespace FluxConfig.Management.Infrastructure.Dal.Migrations;

[Migration(version:20250322, TransactionBehavior.Default)]
public class InitSchema: Migration
{
    public override void Up()
    {
        Create.Table("user_credentials")
            .WithColumn("id").AsInt64().PrimaryKey("user_credentials_pk").Identity()
            .WithColumn("username").AsString(100).NotNullable()
            .WithColumn("email").AsString(100).NotNullable()
            .WithColumn("password").AsString(255).NotNullable();
        
        
        Create.Table("user_sessions")
            .WithColumn("id").AsString(255).PrimaryKey("user_sessions_pk").Unique()
            .WithColumn("user_id").AsInt64().NotNullable()
            .WithColumn("expiration_date").AsDateTimeOffset().NotNullable();


        Create.Table("user_global_roles")
            .WithColumn("id").AsInt64().PrimaryKey("user_global_roles_pk").Identity()
            .WithColumn("user_id").AsInt64().NotNullable()
            .WithColumn("role").AsCustom("user_global_role_enum").NotNullable();

        Create.ForeignKey("FK_user_session")
            .FromTable("user_sessions").ForeignColumn("user_id")
            .ToTable("user_credentials").PrimaryColumn("id")
            .OnDelete(Rule.Cascade);
        
        Create.ForeignKey("FK_user_g_role")
            .FromTable("user_global_roles").ForeignColumn("user_id")
            .ToTable("user_credentials").PrimaryColumn("id")
            .OnDelete(Rule.Cascade);
        
        Create.Index("user_credentials_email_index")
            .OnTable("user_credentials")
            .OnColumn("email")
            .Unique();

        Create.Index("user_global_roles_uid_index")
            .OnTable("user_global_roles")
            .OnColumn("user_id");
    }

    public override void Down()
    {
        Delete.Table("user_global_roles");
        Delete.Table("user_sessions");
        Delete.Table("user_credentials");
    }
}