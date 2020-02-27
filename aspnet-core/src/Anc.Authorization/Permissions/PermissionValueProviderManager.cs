using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anc.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Anc.Authorization.Permissions
{
    public class PermissionValueProviderManager : IPermissionValueProviderManager, ISingletonDependency
    {
        public IReadOnlyList<IPermissionValueProvider> ValueProviders => _lazyProviders.Value;
        private readonly Lazy<List<IPermissionValueProvider>> _lazyProviders;

        protected AncPermissionOptions Options { get; }

        public PermissionValueProviderManager(
            IServiceProvider serviceProvider,
            IOptions<AncPermissionOptions> options)
        {
            Options = options.Value;
            //_lazyProviders = new Lazy<List<IPermissionValueProvider>>(
            //    () => Options
            //        .ValueProviders
            //        .Select(c => serviceProvider.GetRequiredService(c) as IPermissionValueProvider)
            //        .ToList(),
            //    true
            //);
        }
    }
}