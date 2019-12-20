using Anc.Application.Services.Dto;
using CacheCow.Server;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using Snow.AuthorityManagement.Core.Authorization.Roles.DomainService;
using System;

namespace Snow.AuthorityManagement.Web.Core.Common.ETag.Role
{
    public class RoleCollectionETagExtractor : ITimedETagExtractor<PagedResultDto<RoleListDto>>
    {
        private readonly ICacheManager<DateTime?> _cache;
        private readonly IRoleManager _roleManager;

        public RoleCollectionETagExtractor(ICacheManager<DateTime?> cache,
          IRoleManager userManager)
        {
            _cache = cache;
            _roleManager = userManager;
        }

        public TimedEntityTagHeaderValue Extract(PagedResultDto<RoleListDto> viewModel)
        {
            if (viewModel == null)
                return null;
            DateTime? time = _cache.Get(AuthorityManagementConsts.RoleLastResponseCache);
            if (time == null)
            {
                time = _roleManager.GetLastModificationTime();
                if (time == null)
                {
                    return null;
                }
            }

            return new TimedEntityTagHeaderValue(new DateTimeOffset(time.Value).ToETagString(_roleManager.GetCount()));
        }

        public TimedEntityTagHeaderValue Extract(object viewModel)
        {
            return Extract(viewModel as PagedResultDto<RoleListDto>);
        }
    }
}