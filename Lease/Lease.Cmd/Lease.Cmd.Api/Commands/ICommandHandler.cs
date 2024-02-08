namespace Lease.Cmd.Api.Commands
{
    public interface ICommandHandler
    {
        Task HandleAsync(CreateLeaseCommand command);

        Task HandleAsync(SendLeaseCommand command);

        Task HandleAsync(SignLeaseCommand command);

        Task HandleAsync(EditLeaseCommand command);

        Task HandleAsync(CancelLeaseCommand command);

        Task HandleAsync(ReplayEventsCommand command);
    }
}
