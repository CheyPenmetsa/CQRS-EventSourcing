using Lease.Query.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lease.Query.Infrastructure.DataAccess
{
    public class ReadDbContext : DbContext
    {
        public ReadDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<LeaseEntity> Leases { get; set; }

        public DbSet<CustomerEntity> Customers { get; set; }
    }
}
