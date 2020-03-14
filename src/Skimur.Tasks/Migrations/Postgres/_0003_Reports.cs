using System.Data;
using ServiceStack.OrmLite.Dapper;
using Skimur.Backend.Sql;
using Skimur.Backend.Postgres.Migrations;

namespace Skimur.Tasks.Migrations.Postgres
{
    public class _0003_Reports : Migration
    {
        public _0003_Reports() : base(MigrationType.Schema, 3)
        {
        }

        public override void Execute(IDbConnectionProvider conn)
        {
            conn.Perform(x =>
            {
                x.Execute("ALTER TABLE posts ADD COLUMN number_of_reports integer NOT NULL DEFAULT 0;");
                x.Execute("ALTER TABLE comments ADD COLUMN number_of_reports integer NOT NULL DEFAULT 0;");

                x.Execute("ALTER TABLE posts ADD COLUMN ignore_reports boolean NOT NULL DEFAULT false;");
                x.Execute("ALTER TABLE comments ADD COLUMN ignore_reports boolean NOT NULL DEFAULT false;");

                x.Execute(@"
                CREATE TABLE reported_comments
                (
                    id uuid NOT NULL,
                    created_at timestamp without time zone NOT NULL,
                    reported_by uuid NOT NULL,
                    reason text,
                    comment uuid,
                    CONSTRAINT reported_comments_pkey PRIMARY KEY (id)
                );");

                x.Execute(@"
                CREATE TABLE reported_posts
                (
                    id uuid NOT NULL,
                    created_date timestamp with time zone NOT NULL,
                    reported_by uuid NOT NULL,
                    reason text,
                    post uuid,
                    CONSTRAINT reported_posts_pkey PRIMARY KEY (id)
                );");
            });
        }

        public override string GetDescription()
        {
            return "Create the tables for post/comment reports. " +
                "Also, add the number_of_reports and ignore_reports columns to posts and comments.";
        }
    }
}
