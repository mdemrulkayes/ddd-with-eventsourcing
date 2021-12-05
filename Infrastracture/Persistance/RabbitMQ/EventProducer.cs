using Domain.Entities;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.Persistance.RabbitMQ
{
    public class EventProducer<TA, TKey> : IDisposable, IEventProducer<TA, TKey> where TA : IAggregateRoot<TKey>
    {
        public async Task DispatchAsync(TA aggregate)
        {
            //TODO: Need to implement
        }

        public void Dispose()
        {
            //TODO: Need to implement
        }
    }
}
