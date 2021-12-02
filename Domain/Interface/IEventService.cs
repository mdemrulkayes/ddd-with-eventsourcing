using Domain.Entities;

namespace Domain.Interface
{
    public interface IEventService<TA, TKey> where TA : class, IAggregateRoot<TKey>
    {
        Task PersistAsync(TA aggragate);
        Task<TA> RehydrateAsync(TKey key);
    }
}
