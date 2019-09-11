using Anc.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Application.Navigation
{
    public interface INavigationProvider : ITransientDependency

    {
        MenuDefinition GetNavigation();
    }
}