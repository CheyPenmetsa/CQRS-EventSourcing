using Lease.Query.Domain.Entities;

namespace Lease.Query.Api.Queries
{
    public interface IQueryHandler
    {
        Task<List<LeaseEntity>> HandleAsync(FindAllLeasesQuery query);

        Task<List<LeaseEntity>> HandleAsync(FindLeaseByIdQuery query);
    }
}
