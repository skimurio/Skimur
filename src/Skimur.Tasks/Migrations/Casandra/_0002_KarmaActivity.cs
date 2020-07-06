﻿using Cassandra;
using Skimur.Backend.Cassandra.Migrations;

namespace Skimur.Tasks.Migrations.Casandra
{
    public class _0002_KarmaActivity : Migration
    {
        public _0002_KarmaActivity() : base(MigrationType.Schema, 2)
        {

        }

        public override void Execute(ISession session)
        {
            session.Execute("CREATE TABLE UserKarma (user_id uuid, sub_type text, karma counter, PRIMARY KEY(user_id, sub_type)) WITH COMPACT STORAGE");
        }

        public override string GetDescription()
        {
            return "Create the table that will keep track of user karma.";
        }
    }
}
