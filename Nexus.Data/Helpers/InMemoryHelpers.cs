using System;
using Microsoft.EntityFrameworkCore;

namespace Nexus.Data.Helpers
{
    public class InMemoryHelpers
    {
        public static DbContextOptions<NexusContext> CreateOptions(string dbName)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NexusContext>();
            optionsBuilder.UseInMemoryDatabase(dbName);
            optionsBuilder.EnableSensitiveDataLogging();

            return optionsBuilder.Options;
        }

        public static DbContextOptions<NexusContext> CreateOptions()
        {
            return CreateOptions(Guid.NewGuid().ToString());
        }
    }
}