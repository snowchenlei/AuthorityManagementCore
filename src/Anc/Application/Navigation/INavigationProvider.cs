using Anc.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Anc.Application.Navigation
{
    public interface INavigationProvider : ITransientDependency
    {
        Task<MenuDefinition> GetNavigationAsync();
    }
}