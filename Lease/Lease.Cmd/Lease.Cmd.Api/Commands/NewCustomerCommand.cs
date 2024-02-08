using CQRS.Core.Commands;

namespace Lease.Cmd.Api.Commands
{
    public class NewCustomerCommand : BaseCommand
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }
    }
}
