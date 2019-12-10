using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Anc.Domain.Entities
{
    /// <inheritdoc/>
    [Serializable]
    public abstract class Entity : Entity<int>
    {
    }

    /// <inheritdoc cref="IEntity{TKey}"/>
    [Serializable]
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        /// <inheritdoc/>
        public virtual TKey Id { get; set; }

        protected Entity()
        {
        }

        protected Entity(TKey id)
        {
            Id = id;
        }
    }
}