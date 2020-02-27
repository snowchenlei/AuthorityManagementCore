using System;
using System.Collections.Generic;
using System.Text;
using Anc.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionRegistrationActionExtensions
    {
        // OnRegistred

        public static void OnRegistred(this IServiceCollection services, Action<IOnServiceRegistredContext> registrationAction)
        {
            GetOrCreateRegistrationActionList(services).Add(registrationAction);
        }

        public static ServiceRegistrationActionList GetRegistrationActionList(this IServiceCollection services)
        {
            return GetOrCreateRegistrationActionList(services);
        }

        private static ServiceRegistrationActionList GetOrCreateRegistrationActionList(IServiceCollection services)
        {
            var actionList = services.GetSingletonInstanceOrNull<IObjectAccessor<ServiceRegistrationActionList>>()?.Value;
            if (actionList == null)
            {
                actionList = new ServiceRegistrationActionList();
                services.AddObjectAccessor(actionList);
            }

            return actionList;
        }
    }
}