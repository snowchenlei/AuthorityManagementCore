using System;
using System.Collections.Generic;
using System.Text;
using Anc.DependencyInjection;
using JetBrains.Annotations;

namespace Anc.DependencyInjection
{
    public class ObjectAccessor<T> : IObjectAccessor<T>
    {
        public T Value { get; set; }

        public ObjectAccessor()
        {
        }

        public ObjectAccessor([CanBeNull] T obj)
        {
            Value = obj;
        }
    }
}