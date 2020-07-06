﻿using Cassandra;
using Skimur.Backend.Cassandra.Migrations;

namespace Skimur.Tasks.Migrations.Casandra
{
    public class _0001_SubActivityMigration : Migration
    {
        public _0001_SubActivityMigration() : base(MigrationType.Schema, 1)
        {

        }

        public override void Execute(ISession session)
        {
            session.Execute("CREATE TABLE ActivityBySub ( subId uuid, accountId uuid, PRIMARY KEY (subId, accountId)) WITH COMPACT STORAGE;");
        }

        public override string GetDescription()
        {
            return "Create the sub activity tracking table.";
        }
    }
}
