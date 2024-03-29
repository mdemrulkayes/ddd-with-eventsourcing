﻿using Domain;
using Domain.Entities;
using Domain.Interface;
using EventStore.ClientAPI;
using Infrastracture.EventStore;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastracture.Persistance
{
    public class EventRepository<TA, TKey> : IEventRepository<TA, TKey> where TA : class, IAggregateRoot<TKey>
    {
        private readonly IEventStoreConnectionWrapper _connection;
        private readonly string _streamName;
        public EventRepository(IEventStoreConnectionWrapper connection)
        {
            _connection = connection;
            _streamName = typeof(TA).Name;

        }
        public async Task AppendAsync(TA aggregate)
        {
            if (aggregate == null)
                throw new ArgumentNullException(nameof(aggregate));
            if (!aggregate.Events.Any())
                return;

            var connection = await _connection.GetConnectionAsync();
            var streamName = GetStreamName(aggregate.Id);

            var firstEvent = aggregate.Events.First();
            var version = firstEvent.AggregateVersion - 1;

            var transaction = await connection.StartTransactionAsync(streamName, version);
            try
            {
                foreach (var @event in aggregate.Events)
                {
                    var eventData = Map(@event);
                    await transaction.WriteAsync(eventData);
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

        }

        public async Task<TA> RehydrateAsync(TKey key)
        {
            var connection = await _connection.GetConnectionAsync();
            var streamName = GetStreamName(key);

            var events = new List<IDomainEvent<TKey>>();

            StreamEventsSlice currentSlice;
            long nextSliceStart = StreamPosition.Start;
            do
            {
                currentSlice = await connection.ReadStreamEventsForwardAsync(streamName, nextSliceStart, 200, false);

                nextSliceStart = currentSlice.NextEventNumber;

                //var data = (IEnumerable<IDomainEvent<TKey>>)currentSlice.Events.Select(@event =>
                //JsonSerializer.Deserialize(Encoding.UTF8.GetString(@event.OriginalEvent.Data),
                //    Type.GetType(Encoding.UTF8.GetString(@event.OriginalEvent.Metadata))));



                events.AddRange(currentSlice.Events.Select(Map));
            } while (!currentSlice.IsEndOfStream);

            if (!events.Any())
                return default;

            var result = BaseAggregateRoot<TA, TKey>.Create(events.OrderBy(e => e.AggregateVersion));

            return result;
        }

        private string GetStreamName(TKey key) => $"{_streamName}_{key}";

        private IDomainEvent<TKey> Map(ResolvedEvent resolvedEvent)
        {
            var meta = System.Text.Json.JsonSerializer.Deserialize<EventMeta>(resolvedEvent.Event.Metadata);
            return this.Deserialize<TKey>(meta.EventType, Encoding.UTF8.GetString(resolvedEvent.Event.Data));
        }

        private static readonly Newtonsoft.Json.JsonSerializerSettings JsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings()
        {
            ConstructorHandling = Newtonsoft.Json.ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = new PrivateSetterContractResolver()
        };

        public IDomainEvent<TKey> Deserialize<TKey>(string type, string data)
        {
            //TODO: cache types
            var eventType = Type.GetType(type);
            if (null == eventType)
                throw new ArgumentOutOfRangeException(nameof(type), $"invalid event type: {type}");

            // as of 01/10/2020, "Deserialization to reference types without a parameterless constructor isn't supported."
            // https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to
            // apparently it's being worked on: https://github.com/dotnet/runtime/issues/29895

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject(data, eventType, JsonSerializerSettings);

            return (IDomainEvent<TKey>)result;
        }

        private static EventData Map(IDomainEvent<TKey> @event)
        {
            var json = JsonSerializer.Serialize((dynamic)@event);
            var data = Encoding.UTF8.GetBytes(json);

            var eventType = @event.GetType();
            var meta = new EventMeta()
            {
                EventType = eventType.AssemblyQualifiedName
            };
            var metaJson = JsonSerializer.Serialize(meta);
            var metadata = Encoding.UTF8.GetBytes(metaJson);

            var eventPayload = new EventData(Guid.NewGuid(), eventType.Name, true, data, metadata);
            return eventPayload;
        }

        private class EventMeta
        {
            public string EventType { get; set; }
        }
    }
}
