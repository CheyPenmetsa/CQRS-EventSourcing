using CQRS.Core.Commands;

namespace Lease.Cmd.Api.Commands
{
    public class SignLeaseCommand : BaseCommand
    {
        public string EmailAddress { get; set; }
    }
}
