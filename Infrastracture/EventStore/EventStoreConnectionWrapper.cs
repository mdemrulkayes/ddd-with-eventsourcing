using EventStore.ClientAPI;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.EventStore
{
    public class EventStoreConnectionWrapper : IEventStoreConnectionWrapper, IDisposable
    {
        private readonly Lazy<Task<IEventStoreConnection>> _connection;
        private readonly ILogger<IEventStoreConnection> _logger;
        private readonly Uri _uri;

        public EventStoreConnectionWrapper(Uri uri, ILogger<IEventStoreConnection> logger)
        {
            _uri = uri;
            _logger = logger;

            _connection = new Lazy<Task<IEventStoreConnection>>(() =>
            {
                return Task.Run(async () =>
                {
                    var connection = ConnectionSetup();

                    await connection.ConnectAsync();

                    return connection;
                });
            });
        }

        private IEventStoreConnection ConnectionSetup()
        {
            var settings = ConnectionSettings.Create()
                 .EnableVerboseLogging()
                 .UseConsoleLogger()
                 .DisableTls()
                .Build();
            var connection = EventStoreConnection.Create(settings, _uri);

            connection.ErrorOccurred += async (s, e) =>
            {
                _logger.LogWarning(e.Exception,
                    $"an error has occurred on the Eventstore connection: {e.Exception.Message} . Trying to reconnect...");
                connection = ConnectionSetup();
                await connection.ConnectAsync();
            };
            connection.Disconnected += async (s, e) =>
            {
                _logger.LogWarning($"The Eventstore connection has dropped. Trying to reconnect...");
                connection = ConnectionSetup();
                await connection.ConnectAsync();
            };
            connection.Closed += async (s, e) =>
            {
                _logger.LogWarning($"The Eventstore connection was closed: {e.Reason}. Opening new connection...");
                connection = ConnectionSetup();
                await connection.ConnectAsync();
            };
            return connection;
        }

        public Task<IEventStoreConnection> GetConnectionAsync()
        {
            return _connection.Value;
        }

        public void Dispose()
        {
            if (_connection.IsValueCreated)
                return;
            _connection.Value.Result.Dispose();
        }
    }
}
