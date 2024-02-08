using Lease.Query.Domain.Entities;
using Lease.Query.Domain.Repositories;

namespace Lease.Query.Api.Queries
{
    public class QueryHandler : IQueryHandler
    {
        private readonly ILeaseRepository _leaseRepository;

        public QueryHandler(ILeaseRepository leaseRepository)
        {
            _leaseRepository = leaseRepository;
        }

        public async Task<List<LeaseEntity>> HandleAsync(FindAllLeasesQuery query)
        {
            return await _leaseRepository.GetAllLeasesAsync();
        }

        public async Task<List<LeaseEntity>> HandleAsync(FindLeaseByIdQuery query)
        {
            var lease = await _leaseRepository.GetByIdAsync(query.LeaseId);
            return new List<LeaseEntity>() { lease };
        }
    }
}
