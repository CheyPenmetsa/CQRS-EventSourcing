using CQRS.Core.Commands;

namespace Lease.Cmd.Api.Commands
{
    public class EditLeaseCommand : BaseCommand
    {
        public string ParkingSpace { get; set; }

        public string ApartmentNumber { get; set; }
    }
}
