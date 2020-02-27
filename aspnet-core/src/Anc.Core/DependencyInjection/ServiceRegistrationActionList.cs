using System;
using System.Collections.Generic;
using System.Text;
using Anc.DependencyInjection;

namespace Anc.DependencyInjection
{
    public class ServiceRegistrationActionList : List<Action<IOnServiceRegistredContext>>
    {
    }
}