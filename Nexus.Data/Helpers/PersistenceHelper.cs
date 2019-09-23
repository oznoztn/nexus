using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Nexus.Data.Helpers
{
    public static class PersistenceHelper
    {
        public static DbContextOptions<NexusContext> BuildOptions(string connectionStringName)
        {
            DbContextOptionsBuilder<NexusContext> optionsBuilder = new DbContextOptionsBuilder<NexusContext>();

            // getting the appsetting.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // defining the database provider
            optionsBuilder.UseSqlServer(config.GetConnectionString(connectionStringName));

            return optionsBuilder.Options;
        }
    }
}