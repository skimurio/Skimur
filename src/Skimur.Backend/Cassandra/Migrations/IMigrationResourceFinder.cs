namespace Skimur.Backend.Cassandra.Migrations
{
    public interface IMigrationResourceFinder
    {
        MigrationResources Find();
    }
}
