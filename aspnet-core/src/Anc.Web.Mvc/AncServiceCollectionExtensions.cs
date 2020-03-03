using System;
using System.Collections.Generic;
using System.Text;
using Anc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Anc.AspNetCore
{
    public static class AncServiceCollectionExtensions
    {
        public static void AddAnc(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
            
        }
    }
}
