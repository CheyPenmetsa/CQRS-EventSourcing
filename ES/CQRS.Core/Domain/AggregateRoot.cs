using CQRS.Core.Events;

namespace CQRS.Core.Domain
{
    public abstract class AggregateRoot
    {
        protected Guid _id { get; set; }

        private readonly List<BaseEvent> _changes = new();

        public Guid Id { 
            get 
            { 
                return _id; 
            } 
        }

        public int Version { get; set; } = -1;

        // Returns all the uncommited events from event store
        public virtual IEnumerable<BaseEvent> GetUncommittedChanges()
        {
            return _changes;
        }

        // Will set the aggregate state after applying all the events to event store
        public virtual void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        // This will get the ApplyChange method from aggregate which inherits aggregateroot and apply the event on that method
        private void ApplyChange(BaseEvent @event, bool isNew)
        {
            var method = this.GetType().GetMethod("Apply", new Type[] { @event.GetType() });

            if(method == null)
            {
                throw new ArgumentNullException(nameof(method),
                    $"No ApplyChange method was found in {this.GetType().Name} class for event {@event.GetType().Name}");
            }

            method.Invoke(this, new object[] { @event });

            // This is very important because some cases we will reconstruct the aggregate from event store.
            // In that case we dont need to add it to the eventstore since we already had those events registered.
            if (isNew)
            {
                _changes.Add(@event);
            }
        }

        protected void RaiseEvent(BaseEvent @event)
        {
            ApplyChange(@event, true);
        }

        public void ReplayEvents(IEnumerable<BaseEvent> events)
        {
            foreach (var @event in events)
            {
                ApplyChange(@event, false);
            }
        }
    }
}
