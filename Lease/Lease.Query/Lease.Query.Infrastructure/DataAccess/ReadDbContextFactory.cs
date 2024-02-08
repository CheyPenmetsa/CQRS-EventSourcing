using Microsoft.EntityFrameworkCore;

namespace Lease.Query.Infrastructure.DataAccess
{
    public class ReadDbContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;

        public ReadDbContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContext = configureDbContext;
        }

        public ReadDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<ReadDbContext> optionsBuilder = new();
            _configureDbContext(optionsBuilder);

            return new ReadDbContext(optionsBuilder.Options);
        }
    }
}
