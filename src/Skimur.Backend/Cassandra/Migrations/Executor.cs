﻿using System;
using System.Diagnostics;
using System.Linq;
using Cassandra;
using Skimur.Backend.Cassandra.Migrations.DB;
using Skimur.Logging;

namespace Skimur.Backend.Cassandra.Migrations
{
    internal class Executor
    {
        private readonly ISession _session;
        private readonly IVersioner _versioner;
        private readonly ILogger<Executor> _logger;

        public Executor(ISession session, IVersioner versioner, ILogger<Executor> logger)
        {
            _session = session;
            _versioner = versioner;
            _logger = logger;
        }

        public bool Execute(MigrationResources resources)
        {
            _logger.Info("Start executing migrations");
            return resources.Migrations.OrderBy(x => x.Version).All(Execute);
        }

        private bool Execute(Migration migration)
        {
            var type = migration.Type;
            var version = migration.Version;
            var dbVersion = _versioner.CurrentVersion(type);

            _logger.Info(string.Format("Database {0} version is : {1}", type, dbVersion));

            if (version <= dbVersion)
            {
                _logger.Warn(string.Format("{0} migration version {1} is less than database version", type, version));
                return true;
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();

            _logger.Info("Executing migration to version " + version);
            try
            {
                migration.Execute(_session);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Failed to execute {0} migration to version {1}", type, version), ex);
                return false;
            }

            sw.Stop();
            _logger.Info(string.Format("migration to version {0} took {1} seconds.", version, sw.Elapsed.TotalSeconds));

            if (!_versioner.SetVersion(migration))
            {
                _logger.Error("Failed to update database version. Leaving...");
                return false;
            }

            return true;
        }
    }
}
