using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace Anc.DependencyInjection
{
    public interface IObjectAccessor<out T>
    {
        [CanBeNull]
        T Value { get; }
    }
}