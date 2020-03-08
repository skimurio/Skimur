using Cassandra;

namespace Skimur.Backend.Cassandra.Migrations
{
    public interface IMigrationEngine
    {
        bool Execute(ISession session, MigrationResources resources);
    }
}
