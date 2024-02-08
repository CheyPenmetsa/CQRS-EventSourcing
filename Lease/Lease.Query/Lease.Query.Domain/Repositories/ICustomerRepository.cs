using CQRS.Core.Domain;
using Lease.Query.Domain.Entities;

namespace Lease.Query.Domain.Repositories
{
    public interface ICustomerRepository : IEntityRepository<CustomerEntity>
    {
        Task<List<CustomerEntity>> GetCustomersByLeaseIdAsync(Guid leaseId);
    }
}
