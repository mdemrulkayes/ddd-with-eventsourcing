using Domain.Entities;
using Domain.Implementation;
using Domain.Interface;
using Infrastracture.EventStore;
using Infrastracture.Persistance;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEvents(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IEventStoreConnectionWrapper>(ctx =>
            {
                var logger = ctx.GetRequiredService<ILogger<EventStoreConnectionWrapper>>();
                return new EventStoreConnectionWrapper(new Uri(connectionString), logger);
            }).AddEventsRepository<Domain.Models.Task, Guid>();

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddEventsService<Domain.Models.Task, Guid>();
            return services;
        }

        private static IServiceCollection AddEventsRepository<TA, TK>(this IServiceCollection services)
        where TA : class, IAggregateRoot<TK>
        {
            return services.AddSingleton<IEventRepository<TA, TK>>(ctx =>
            {
                var connectionWrapper = ctx.GetRequiredService<IEventStoreConnectionWrapper>();
                return new EventRepository<TA, TK>(connectionWrapper);
            });
        }

        private static IServiceCollection AddEventsService<TA, TK>(this IServiceCollection services)
            where TA : class, IAggregateRoot<TK>
        {
            return services.AddSingleton<IEventService<TA, TK>>(ctx =>
            {
                //var eventsProducer = ctx.GetRequiredService<IEventProducer<TA, TK>>();
                var eventsRepo = ctx.GetRequiredService<IEventRepository<TA, TK>>();

                //return new EventService<TA, TK>(eventsRepo, eventsProducer);
                return new EventService<TA, TK>(eventsRepo);
            });
        }
    }
}
