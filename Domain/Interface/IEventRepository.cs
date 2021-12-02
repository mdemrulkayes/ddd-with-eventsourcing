using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IEventRepository<TA, TKey> where TA: class, IAggregateRoot<TKey>
    {
        Task AppendAsync(TA aggregate);
        Task<TA> RehydrateAsync(TKey key);
    }
}
