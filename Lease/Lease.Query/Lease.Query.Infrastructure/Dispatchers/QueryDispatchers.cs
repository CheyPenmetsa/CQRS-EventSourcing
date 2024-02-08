using CQRS.Core.Infrastructure;
using CQRS.Core.Queries;
using Lease.Query.Domain.Entities;

namespace Lease.Query.Infrastructure.Dispatchers
{
    public class QueryDispatchers : IQueryDispatcher<LeaseEntity>
    {
        private readonly Dictionary<Type, Func<BaseQuery, Task<List<LeaseEntity>>>> _handlers = new();

        public void RegisterQueryHandler<TQuery>(Func<TQuery, Task<List<LeaseEntity>>> handler) where TQuery : BaseQuery
        {
            if (_handlers.ContainsKey(typeof(TQuery)))
            {
                throw new IndexOutOfRangeException($"Cannot register same query handler again.");
            }
            _handlers.Add(typeof(TQuery), x=> handler((TQuery)x));
        }

        public async Task<List<LeaseEntity>> SendQueryAsync(BaseQuery query)
        {
            if (_handlers.TryGetValue(query.GetType(), out Func<BaseQuery, Task<List<LeaseEntity>>> handler))
            {
                return await handler(query);
            }

            throw new ArgumentNullException(nameof(handler), "No query handler was registered!");
        }
    }
}
