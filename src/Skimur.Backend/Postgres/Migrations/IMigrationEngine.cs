using Skimur.Backend.Sql;

namespace Skimur.Backend.Postgres.Migrations
{
    public interface IMigrationEngine
    {
        bool Execute(IDbConnectionProvider conn, MigrationResources resources);
    }
}
