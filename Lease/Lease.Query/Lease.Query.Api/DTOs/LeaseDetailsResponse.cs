using Lease.Common.DTOs;
using Lease.Query.Domain.Entities;

namespace Lease.Query.Api.DTOs
{
    public class LeaseDetailsResponse
    {
        public List<LeaseEntity> Leases { get; set; }
    }
}
