﻿namespace Skimur.Backend.Postgres.Migrations.DB
{
    internal interface IVersioner
    {
        int CurrentVersion(MigrationType type);

        bool SetVersion(Migration migration);
    }
}
