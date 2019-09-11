using Anc.Application.Services.Dto;
using CacheCow.Server;
using CacheManager.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Core.Authorization.Users.DomainService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Web.Core.Common.ETag.User
{
    public class TimedETagQueryUserRepository : ITimedETagQueryProvider<PagedResultDto<UserListDto>>, ITimedETagQueryProvider<GetUserForEditOutput>
    {
        private readonly ICacheManager<DateTime?> _cache;
        private readonly IUserManager _userManager;

        public TimedETagQueryUserRepository(ICacheManager<DateTime?> cache
            , IUserManager userManager)
        {
            _cache = cache;
            _userManager = userManager;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Task<TimedEntityTagHeaderValue> QueryAsync(HttpContext context)
        {
            int? id = null;
            var routeData = context.GetRouteData();
            if (routeData.Values.ContainsKey("id"))
                id = Convert.ToInt32(routeData.Values["id"]);

            if (id.HasValue)
            {
                DateTime? time = _cache.Get(string.Format(AuthorityManagementConsts.UserResponseCache, id));
                if (time.HasValue)
                {
                    return Task.FromResult(new TimedEntityTagHeaderValue(new DateTimeOffset(time.Value).ToETagString()));
                }
                else
                {
                    return Task.FromResult((TimedEntityTagHeaderValue)null);
                }
            }
            else // all cars
            {
                DateTime? time = _cache.Get(AuthorityManagementConsts.UserLastResponseCache);
                if (time == null)
                {
                    return Task.FromResult((TimedEntityTagHeaderValue)null);
                }
                return Task.FromResult(new TimedEntityTagHeaderValue(new DateTimeOffset(time.Value).ToETagString(_userManager.GetCount())));
            }
        }
    }
}