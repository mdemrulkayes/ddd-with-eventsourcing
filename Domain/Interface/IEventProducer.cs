using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IEventProducer<in TA, in TKey> where TA : IAggregateRoot<TKey>
    {
        Task DispatchAsync(TA aggregate);
    }
}
