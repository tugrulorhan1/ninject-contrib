
using System.Configuration;
using NSpecify.Framework;
using RikMigrations;

namespace LoggingDemo.Migrations
{
    [Context]
    public class RunMigrations
    {
        [Specification]
        public void UpgradeToMax()
        {
            DbProvider.DefaultConnectionString =
                ConfigurationManager.ConnectionStrings["LoggingConnection"].ConnectionString;

            MigrationManager.UpgradeMax(typeof(RunMigrations).Assembly);
        }
    }
}