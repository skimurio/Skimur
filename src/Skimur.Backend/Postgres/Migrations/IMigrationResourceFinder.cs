namespace Skimur.Backend.Postgres.Migrations
{
    public interface IMigrationResourceFinder
    {
        MigrationResources Find();
    }
}
