using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
    {
        protected BaseEntity() { }

        protected BaseEntity(TKey id) => Id = id;

        public TKey Id { get; protected set; }
    }
}
