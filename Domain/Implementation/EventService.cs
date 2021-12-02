using Domain.Entities;
using Domain.Interface;

namespace Domain.Implementation
{
    public class EventService<TA, TKey> : IEventService<TA, TKey> where TA : class, IAggregateRoot<TKey>
    {
        public Task PersistAsync(TA aggragate)
        {
            throw new NotImplementedException();
        }

        public Task<TA> RehydrateAsync(TKey key)
        {
            throw new NotImplementedException();
        }
    }
}
