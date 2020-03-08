using System;
using System.Linq;
using Cassandra;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using ServiceStack.Text;
using Skimur.Logging;

namespace Skimur.Backend.Cassandra.Migrations.DB
{
    internal class Versioner : IVersioner
    {
        private IMapper _mapper;
        private ISession _session;
        private readonly ILogger<Version> _logger;
        private static bool _didRegisterMapping = false;
        private static object _registrationLock = new object();

        public Versioner(ISession session, ILogger<Version> logger)
        {
            _logger = logger;
            _session = session;

            _logger.Info("Define global mappings");
            lock (_registrationLock)
            {
                if (!_didRegisterMapping)
                {
                    MappingConfiguration.Global.Define<PocoMapper>();
                    _didRegisterMapping = true;
                }
            }

            _logger.Info("Create mapper and table instances");
            _mapper = new Mapper(_session);

            var table = new Table<DatabaseVersion>(_session);
            table.CreateIfNotExists();
        }

        public int CurrentVersion(MigrationType type)
        {
            var versions = _mapper.Fetch<DatabaseVersion>("where type = ?", (int)type).ToList();

            if (versions.Count == 0)
                return 0;

            return versions.Max(x => x.Version);
        }

        public bool SetVersion(Migration migration)
        {
            try
            {
                _logger.Info("Updating database version to " + migration.Version);
                _mapper.Insert(new DatabaseVersion
                {
                    Type = migration.Type,
                    Version = migration.Version,
                    Description = migration.GetDescription(),
                    Timestamp = DateTime.Now.ToUnixTime()
                });
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
