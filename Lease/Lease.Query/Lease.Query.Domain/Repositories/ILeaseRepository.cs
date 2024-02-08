using CQRS.Core.Domain;
using Lease.Query.Domain.Entities;

namespace Lease.Query.Domain.Repositories
{
    public interface ILeaseRepository : IEntityRepository<LeaseEntity>
    {
        Task CancelLease(Guid leaseId);

        Task<List<LeaseEntity>> GetAllLeasesAsync();

        Task<LeaseEntity?> GetLeaseByCustomerEmailAsync(string customerEmail);
    }
}
