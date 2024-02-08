using Lease.Query.Domain.Entities;
using Lease.Query.Domain.Repositories;
using Lease.Query.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Lease.Query.Infrastructure.Repositories
{
    public class LeaseRepository : ILeaseRepository
    {
        private readonly ReadDbContextFactory _contextFactory;

        public LeaseRepository(ReadDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CancelLease(Guid leaseId)
        {
            using var dbContext = _contextFactory.CreateDbContext();
            var lease = await GetByIdAsync(leaseId);
            if (lease == null)
                return;

            lease.Cancelled = true;
            await dbContext.SaveChangesAsync();
        }

        public async Task CreateAsync(LeaseEntity entity)
        {
            using var dbContext = _contextFactory.CreateDbContext();
            dbContext.Leases.Add(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<LeaseEntity>> GetAllLeasesAsync()
        {
            using var dbContext = _contextFactory.CreateDbContext();
            return await dbContext.Leases.AsNoTracking().Include(x => x.Residents).AsNoTracking().ToListAsync();
        }

        public async Task<LeaseEntity?> GetByIdAsync(Guid entityId)
        {
            using var dbContext = _contextFactory.CreateDbContext();
            return await dbContext.Leases.Include(x => x.Residents).FirstOrDefaultAsync(x => x.LeaseId.Equals(entityId));
        }

        public async Task<LeaseEntity?> GetLeaseByCustomerEmailAsync(string customerEmail)
        {
            using var dbContext = _contextFactory.CreateDbContext();
            return await dbContext.Customers.Include(x=>x.Lease)
                .Where(x=>x.EmailAddress.Equals(customerEmail) && !x.Lease.Cancelled)
                .Select(x=>x.Lease).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(LeaseEntity entity)
        {
            using var dbContext = _contextFactory.CreateDbContext();
            dbContext.Leases.Update(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}
