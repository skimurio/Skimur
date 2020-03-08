﻿using Skimur.Backend.Sql;

namespace Skimur.Backend.Postgres.Migrations
{
    public abstract class Migration
    {
        protected Migration(MigrationType type, int version)
        {
            Type = type;
            Version = version;
        }

        public MigrationType Type { get; private set; }

        public int Version { get; private set; }

        public abstract void Execute(IDbConnectionProvider conn);

        public abstract string GetDescription();
    }
}
