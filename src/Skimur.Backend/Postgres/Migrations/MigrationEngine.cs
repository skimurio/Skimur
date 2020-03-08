using System;
using Skimur.Backend.Sql;
using Skimur.Backend.Postgres.Migrations.DB;
using Skimur.Logging;

namespace Skimur.Backend.Postgres.Migrations
{
    public class MigrationEngine : IMigrationEngine
    {
        private readonly ILogger<MigrationEngine> _logger;

        public MigrationEngine(ILogger<MigrationEngine> logger)
        {
            _logger = logger;
        }

        public bool Execute(IDbConnectionProvider conn, MigrationResources resources)
        {
            _logger.Info("Initialize database versioner");
            var versioner = new Versioner(conn, new Logger<Version>());

            _logger.Info("Initialize executor");
            var executor = new Executor(conn, versioner, new Logger<Executor>());

            _logger.Info("Execute migrations");
            return executor.Execute(resources);
        }
    }
}
