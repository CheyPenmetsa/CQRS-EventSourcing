using CQRS.Core.Queries;

namespace Lease.Query.Api.Queries
{
    public class FindLeaseByIdQuery : BaseQuery
    {
        public Guid LeaseId { get; set; }
    }
}
