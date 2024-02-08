using Lease.Query.Domain.Entities;
using Lease.Query.Domain.Repositories;
using Lease.Query.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Lease.Query.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ReadDbContextFactory _contextFactory;

        public CustomerRepository(ReadDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateAsync(CustomerEntity entity)
        {
            using var dbContext = _contextFactory.CreateDbContext();
            dbContext.Customers.Add(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<CustomerEntity?> GetByIdAsync(Guid entityId)
        {
            using var dbContext = _contextFactory.CreateDbContext();
            return await dbContext.Customers.Include(x => x.Lease).FirstOrDefaultAsync(x => x.CustomerId.Equals(entityId));
        }

        public async  Task<List<CustomerEntity>> GetCustomersByLeaseIdAsync(Guid leaseId)
        {
            using var dbContext = _contextFactory.CreateDbContext();
            return await dbContext.Customers.Include(x => x.Lease).Where(x => x.LeaseId.Equals(leaseId)).ToListAsync();
        }

        public async Task UpdateAsync(CustomerEntity entity)
        {
            using var dbContext = _contextFactory.CreateDbContext();
            dbContext.Customers.Update(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}
