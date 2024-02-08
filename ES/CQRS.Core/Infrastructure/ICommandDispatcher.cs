using CQRS.Core.Commands;

namespace CQRS.Core.Infrastructure
{
    public interface ICommandDispatcher
    {
        void RegisterCommandHandler<T>(Func<T, Task> handler) where T : BaseCommand;

        Task SendCommandAsync(BaseCommand command);
    }
}
