﻿using System;
using System.Linq;
using ServiceStack.OrmLite;
using ServiceStack.Text;
using Skimur.Backend.Sql;
using Skimur.Logging;

namespace Skimur.Backend.Postgres.Migrations.DB
{
    internal class Versioner : IVersioner
    {
        private readonly IDbConnectionProvider _conn;
        private readonly ILogger<Version> _logger;

        public Versioner(IDbConnectionProvider connectionProvider, ILogger<Version> logger)
        {
            _conn = connectionProvider;
            _logger = logger;

            _logger.Info("Create mapper and table instances");
            _conn.Perform(conn =>
            {
                conn.ExecuteSql(@"CREATE TABLE IF NOT EXISTS database_version
                    (
                        type text NOT NULL,
                        version integer NOT NULL,
                        timestamp bigint NOT NULL,
                        description text
                    )");
            });
        }

        public int CurrentVersion(MigrationType type)
        {
            var dbVersion = _conn.Perform(conn => conn.Select(conn.From<DatabaseVersion>()
                                .Where(x => x.Type == type)
                                .OrderByDescending(x => x.Version)
                                .Take(1)))
                                .FirstOrDefault();

            if (dbVersion == null)
            {
                _logger.Warn("No entries in database version table. Defaulting version value to 0.");
                return 0;
            }

            return dbVersion.Version;
        }

        public bool SetVersion(Migration migration)
        {
            try
            {
                _logger.Info("Updating database version to " + migration.Version);
                _conn.Perform(conn => conn.Insert(new DatabaseVersion
                {
                    Type = migration.Type,
                    Version = migration.Version,
                    Description = migration.GetDescription(),
                    Timestamp = DateTimeExtensions.ToUnixTime(DateTime.Now)
                }));
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to execute insert of migration details into database version table", ex);
                return false;
            }

            return true;
        }
    }
}
