using Anc.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Domain.Repositories
{
    public interface IRepository : ITransientDependency
    {
    }
}