using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.EventStore
{
    public interface IEventStoreConnectionWrapper
    {
        Task<IEventStoreConnection> GetConnectionAsync();
    }
}
