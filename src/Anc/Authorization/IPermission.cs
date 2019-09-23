using Anc.Dependency;
using Anc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Authorization
{
    public interface IPermission : ITransientDependency
    {
        string Name { get; set; }
    }
}