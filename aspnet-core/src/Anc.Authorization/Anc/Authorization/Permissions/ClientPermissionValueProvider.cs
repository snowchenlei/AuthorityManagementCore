using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Anc.Security.Claims;

namespace Anc.Authorization.Permissions
{
    public class ClientPermissionValueProvider : PermissionValueProvider
    {
        public const string ProviderName = "Client";

        public override string Name => ProviderName;

        public ClientPermissionValueProvider(IPermissionStore permissionStore)
            : base(permissionStore)
        {
        }

        public override async Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
        {
            var clientId = context.Principal?.FindFirst(AncClaimTypes.ClientId)?.Value;

            if (clientId == null)
            {
                return PermissionGrantResult.Undefined;
            }

            return await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, clientId)
                ? PermissionGrantResult.Granted
                : PermissionGrantResult.Undefined;
        }
    }
}