using CQRS.Core.Commands;

namespace Lease.Cmd.Api.Commands
{
    public class CancelLeaseCommand : BaseCommand
    {
        public string UserName { get; set; }
    }
}
