using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public abstract class BaseAggregateRoot<TA, TKey> : BaseEntity<TKey>, IAggregateRoot<TKey> where TA : class, IAggregateRoot<TKey>
    {
        private readonly Queue<IDomainEvent<TKey>> _events = new Queue<IDomainEvent<TKey>>();
        protected BaseAggregateRoot() { }
        protected BaseAggregateRoot(TKey id) : base(id) { }
        public long Version { get; private set; }

        public IReadOnlyCollection<IDomainEvent<TKey>> Events => _events.ToImmutableArray();

        protected void AddEvent(IDomainEvent<TKey> @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));
            _events.Enqueue(@event);
            this.Apply(@event);
            this.Version++;
        }

        protected abstract void Apply(IDomainEvent<TKey> @event);
        public void ClearEvents()
        {
            _events.Clear();
        }
    }
}
