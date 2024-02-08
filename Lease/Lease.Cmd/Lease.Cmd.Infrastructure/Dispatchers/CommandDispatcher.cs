using CQRS.Core.Commands;
using CQRS.Core.Infrastructure;

namespace Lease.Cmd.Infrastructure.Dispatchers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly Dictionary<Type, Func<BaseCommand, Task>> _commandHandlers = new();

        public void RegisterCommandHandler<T>(Func<T, Task> handler) where T : BaseCommand
        {
            if (_commandHandlers.ContainsKey(typeof(T)))
            {
                throw new IndexOutOfRangeException("Handler already registered");
            }
            _commandHandlers.Add(typeof(T), x => handler((T)x));
        }

        public async Task SendCommandAsync(BaseCommand command)
        {
            if(_commandHandlers.TryGetValue(command.GetType(), out var commandHandler))
            {
                await commandHandler(command);
            }
            else
            {
                throw new ArgumentNullException($"Did not find any handler for command: {command.GetType()}");
            }
        }
    }
}
