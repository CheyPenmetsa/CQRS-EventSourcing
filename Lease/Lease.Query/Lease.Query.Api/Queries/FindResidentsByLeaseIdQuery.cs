using CQRS.Core.Queries;

namespace Lease.Query.Api.Queries
{
    public class FindResidentsByLeaseIdQuery : BaseQuery
    {
        public Guid LeaseId { get; set; }
    }
}
