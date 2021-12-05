using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public abstract class BaseDomainEvent<TA, TKey> : IDomainEvent<TKey> where TA : IAggregateRoot<TKey>
    {
        protected BaseDomainEvent() { }
        public BaseDomainEvent(TA aggragate)
        {
            if (aggragate is null)
                throw new ArgumentNullException(nameof(aggragate));

            this.AggegateId = aggragate.Id;
            this.AggregateVersion = aggragate.Version;
            this.TimeStamp = DateTime.Now;
        }
        public long AggregateVersion { get; private set; }

        public TKey AggegateId { get; private set; }

        public DateTime TimeStamp { get; private set; }
    }
}
