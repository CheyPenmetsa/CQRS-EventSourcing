using CQRS.Core.Handlers;
using Lease.Cmd.Domain.Aggregates;

namespace Lease.Cmd.Api.Commands
{
    public class CommandHandler : ICommandHandler
    {
        private IEventSourcingHandler<LeaseAggregate> _leaseEventSourcingHandler;

        public CommandHandler(IEventSourcingHandler<LeaseAggregate> leaseEventSourcingHandler)
        {
            _leaseEventSourcingHandler = leaseEventSourcingHandler;
        }

        public async Task HandleAsync(CreateLeaseCommand command)
        {
            var leaseAggregate = new LeaseAggregate(command.Id,
                command.CustomerEmails,
                command.FloorPlan,
                command.LeaseStartDate,
                command.LeaseTermLengthInMonths,
                command.RentAmount,
                command.ApartmentNumber,
                command.ParkingSpace);
            await _leaseEventSourcingHandler.SaveAsync(leaseAggregate);
        }

        public async Task HandleAsync(SendLeaseCommand command)
        {
            var leaseAggregate = await _leaseEventSourcingHandler.GetByIdAsync(command.Id);
            leaseAggregate.SendLease();
            await _leaseEventSourcingHandler.SaveAsync(leaseAggregate);
        }

        public async Task HandleAsync(SignLeaseCommand command)
        {
            var leaseAggregate = await _leaseEventSourcingHandler.GetByIdAsync(command.Id);
            leaseAggregate.SignLease(command.EmailAddress);
            await _leaseEventSourcingHandler.SaveAsync(leaseAggregate);
        }

        public async Task HandleAsync(EditLeaseCommand command)
        {
            var leaseAggregate = await _leaseEventSourcingHandler.GetByIdAsync(command.Id);
            leaseAggregate.EditLease(command.ApartmentNumber, command.ParkingSpace);
            await _leaseEventSourcingHandler.SaveAsync(leaseAggregate);
        }

        public async Task HandleAsync(CancelLeaseCommand command)
        {
            var leaseAggregate = await _leaseEventSourcingHandler.GetByIdAsync(command.Id);
            leaseAggregate.CancelLease(command.UserName);
            await _leaseEventSourcingHandler.SaveAsync(leaseAggregate);
        }

        public async Task HandleAsync(ReplayEventsCommand command)
        {
            await _leaseEventSourcingHandler.ReplayEventsAsync(command.Id);
        }
    }
}
