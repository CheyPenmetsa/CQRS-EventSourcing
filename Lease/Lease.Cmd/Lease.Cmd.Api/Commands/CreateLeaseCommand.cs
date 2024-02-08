using CQRS.Core.Commands;

namespace Lease.Cmd.Api.Commands
{
    public class CreateLeaseCommand : BaseCommand
    {
        public List<string> CustomerEmails { get; set; }

        public string FloorPlan { get; set; }

        public DateTime LeaseStartDate { get; set; }

        public int LeaseTermLengthInMonths { get; set; }

        public double RentAmount { get; set; }

        public string ParkingSpace { get; set; }

        public string ApartmentNumber { get; set; }
    }
}
