using System.Data;
using ServiceStack.OrmLite.Dapper;
using Skimur.Backend.Sql;
using Skimur.Backend.Postgres.Migrations;

namespace Skimur.Tasks.Migrations.Postgres
{
    public class _0002_Messages : Migration
    {
        public _0002_Messages() : base(MigrationType.Schema, 2)
        {

        }

        public override void Execute(IDbConnectionProvider conn)
        {
            conn.Perform(x =>
            {
                x.Execute(@"
                CREATE TABLE messages
                (
                    id uuid NOT NULL,
                    created_at timestamp without time zone NOT NULL,
                    type integer,
                    parent_id uuid,
                    first_message uuid,
                    deleted boolean,
                    author_id uuid NOT NULL,
                    author_ip text,
                    is_new boolean,
                    to_user uuid,
                    to_sub uuid,
                    from_sub uuid,
                    subject text,
                    body text,
                    body_formatted text,
                    post uuid NULL,
                    comment uuid NULL,
                    CONSTRAINT messages_pkey PRIMARY KEY (id)
                );");

                x.Execute(@"
                    CREATE INDEX messages_createdat_index ON messages(created_at);
                ");
            });
        }

        public override string GetDescription()
        {
            return "Create the messages table.";
        }
    }
}
