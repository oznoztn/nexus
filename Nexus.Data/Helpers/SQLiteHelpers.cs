using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Nexus.Data.Helpers
{
    public class SQLiteHelpers
    {
        public static DbContextOptions<NexusContext> CreateOptions()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = ":memory:"
            };

            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);
            connection.Open();

            var builder = new DbContextOptionsBuilder<NexusContext>();
            builder.UseSqlite(connection);
            builder.EnableSensitiveDataLogging();

            return builder.Options;
        }        
    }
}