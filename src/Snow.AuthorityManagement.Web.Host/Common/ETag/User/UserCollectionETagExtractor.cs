using Anc.Application.Services.Dto;
using CacheCow.Server;
using CacheManager.Core;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Core.Authorization.Users.DomainService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Web.Core.Common.ETag.User
{
    public class UserCollectionETagExtractor : ITimedETagExtractor<PagedResultDto<UserListDto>>
    {
        private readonly ICacheManager<DateTime?> _cache;
        private readonly IUserManager _userManager;

        public UserCollectionETagExtractor(ICacheManager<DateTime?> cache,
          IUserManager userManager)
        {
            _cache = cache;
            _userManager = userManager;
        }

        public TimedEntityTagHeaderValue Extract(PagedResultDto<UserListDto> viewModel)
        {
            if (viewModel == null)
                return null;
            DateTime? time = _cache.Get(AuthorityManagementConsts.UserLastResponseCache);
            if (time == null)
            {
                time = _userManager.GetLastModificationTime();
                if (time == null)
                {
                    return null;
                }
            }

            return new TimedEntityTagHeaderValue(new DateTimeOffset(time.Value).ToETagString(_userManager.GetCount()));
        }

        public TimedEntityTagHeaderValue Extract(object viewModel)
        {
            return Extract(viewModel as PagedResultDto<UserListDto>);
        }
    }
}