using System.Data;
using ServiceStack.OrmLite.Dapper;
using Skimur.Backend.Sql;
using Skimur.Backend.Postgres.Migrations;

namespace Skimur.Tasks.Migrations.Postgres
{
    public class _0004_Badges : Migration
    {
        public _0004_Badges() : base(MigrationType.Schema, 4)
        {
        }

        public override void Execute(IDbConnectionProvider conn)
        {
            conn.Perform(x =>
            {
               
                x.Execute(@"
                CREATE TABLE badges
                (
                    id uuid NOT NULL,
                    name text,
                    description text,
                    icon text,
                    CONSTRAINT badges_pkey PRIMARY KEY (id)
                );");

                x.Execute(@"
                CREATE TABLE user_badges
                (
                    user_id uuid NOT NULL REFERENCES users(id),
                    badge_id uuid NOT NULL REFERENCES badges(id)
                );");

            });
        }

        public override string GetDescription()
        {
            return "Create the tables for badges and user badges.";
        }
    }
}
