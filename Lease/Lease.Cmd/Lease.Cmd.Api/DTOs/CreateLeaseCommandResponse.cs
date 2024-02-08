using Lease.Common.DTOs;

namespace Lease.Cmd.Api.DTOs
{
    public class CreateLeaseCommandResponse : CommandResponse
    {
        public Guid Id { get; set; }
    }
}
