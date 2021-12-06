using Domain.Entities;
using Domain.Interface;

namespace Domain.Implementation
{
    public class EventService<TA, TKey> : IEventService<TA, TKey> where TA : class, IAggregateRoot<TKey>
    {
        private readonly IEventRepository<TA, TKey> _repository;
        //private readonly IEventProducer<TA, TKey> _producer;

        //public EventService(IEventRepository<TA, TKey> repository, IEventProducer<TA, TKey> producer)
        //{
        //    _repository = repository;
        //    _producer = producer;
        //}

        public EventService(IEventRepository<TA, TKey> repository)
        {
            _repository = repository;
        }

        public async Task PersistAsync(TA aggragate)
        {
            if (aggragate == null)
                throw new ArgumentNullException(nameof(aggragate));

            if (!aggragate.Events.Any())
                return;

            await _repository.AppendAsync(aggragate);
            //Need to dispatch the Event into any Service Bus such as Azure, Kafka, RabbitMQ
            //await _producer.DispatchAsync(aggragate);

            aggragate.ClearEvents();
        }

        public async Task<TA> RehydrateAsync(TKey key)
        {
            return await _repository.RehydrateAsync(key);
        }
    }
}
