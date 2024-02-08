using CQRS.Core.Entities;

namespace CQRS.Core.Domain
{
    public interface IEntityRepository<T> where T : BaseEntity
    {
        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task<T?> GetByIdAsync(Guid entityId);
    }
}
