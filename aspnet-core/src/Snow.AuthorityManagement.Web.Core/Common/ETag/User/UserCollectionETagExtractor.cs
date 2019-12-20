using Anc.Application.Services.Dto;
using CacheCow.Server;
using Microsoft.Extensions.Caching.Distributed;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Core.Authorization.Roles.DomainService;
using System;

namespace Snow.AuthorityManagement.Web.Core.Common.ETag.User
{
    public class UserCollectionETagExtractor : ITimedETagExtractor<PagedResultDto<UserListDto>>
    {
        private readonly IDistributedCache _cache;
        private readonly IRoleManager _userManager;

        public UserCollectionETagExtractor(IDistributedCache cache,
          IRoleManager userManager)
        {
            _cache = cache;
            _userManager = userManager;
        }

        public TimedEntityTagHeaderValue Extract(PagedResultDto<UserListDto> viewModel)
        {
            if (viewModel == null)
                return null;
            DateTime? time;
            string temp = _cache.GetString(AuthorityManagementConsts.UserLastResponseCache);
            if (temp == null)
            {
                time = _userManager.GetLastModificationTime();
                if (time == null)
                {
                    return null;
                }
            }
            else
            {
                if (DateTime.TryParse(temp, out DateTime tempTime))
                {
                    time = tempTime;
                }
                else
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