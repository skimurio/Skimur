using System.Data;
using ServiceStack.OrmLite.Dapper;
using Skimur.Backend.Sql;
using Skimur.Backend.Postgres.Migrations;

namespace Skimur.Tasks.Migrations.Postgres
{
    public class _0005_Wiki : Migration
    {
        public _0005_Wiki() : base(MigrationType.Schema, 5)
        {
        }

        public override void Execute(IDbConnectionProvider conn)
        {
            conn.Perform(x =>
            {
               
                x.Execute(@"
                CREATE TABLE wiki_pages
                (
                    id uuid NOT NULL,
                    sub_id uuid NOT NULL REFERENCES users(id),
                    name text,
                    description text,
                    content text,
                    version_number integer NOT NULL,
                    revision_id uuid,
                    created_at timestamp with time zone NOT NULL,
                    updated_at timestamp with time zone,
                    deleted_at timestamp with time zone,
                    CONSTRAINT wiki_pages_pkey PRIMARY KEY (id)
                );");

                x.Execute(@"
                CREATE TABLE wiki_page_revisions
                (
                    id uuid NOT NULL,
                    page_id uuid NOT NULL REFERENCES wiki_pages(id),
                    content text,
                    created_at timestamp with time zone NOT NULL,
                    CONSTRAINT wiki_page_revisions_pkey PRIMARY KEY (id)
                );");

            });
        }

        public override string GetDescription()
        {
            return "Create the tables for the wiki system";
        }
    }
}
