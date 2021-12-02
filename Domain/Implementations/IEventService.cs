using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementations
{
    public interface IEventService<TA, TKey> where TA : class, IAggregateRoot<TKey>
    {
        Task PersistAsync(TA aggragate);
        Task<TA> RehydrateAsync(TKey key);
    }
}
