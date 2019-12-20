using Anc.Application.Services.Dto;
using CacheCow.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Distributed;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Core.Authorization.Roles.DomainService;
using Snow.AuthorityManagement.Core.Authorization.Users.DomainService;
using Snow.AuthorityManagement.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Web.Core.Common.ETag.User
{
    public class TimedETagQueryUserRepository : ITimedETagQueryProvider<PagedResultDto<UserListDto>>, ITimedETagQueryProvider<GetUserForEditOutput>
    {
        private readonly IDistributedCache _cache;
        private readonly IRoleManager _userManager;

        public TimedETagQueryUserRepository(IDistributedCache cache
            , IRoleManager userManager)
        {
            _cache = cache;
            _userManager = userManager;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public async Task<TimedEntityTagHeaderValue> QueryAsync(HttpContext context)
        {
            int? id = null;
            var routeData = context.GetRouteData();
            if (routeData.Values.ContainsKey("id"))
            {
                id = Convert.ToInt32(routeData.Values["id"]);
            }

            if (id.HasValue)
            {
                string time = await _cache.GetStringAsync(string.Format(AuthorityManagementConsts.UserResponseCache, id));
                if (time == null)
                {
                    return new TimedEntityTagHeaderValue(new DateTimeOffset(Convert.ToDateTime(time)).ToETagString());
                }
                else
                {
                    return null;
                }
            }
            else // all cars
            {
                string time = await _cache.GetStringAsync(AuthorityManagementConsts.UserLastResponseCache);
                if (time == null)
                {
                    return null;
                }
                return new TimedEntityTagHeaderValue(new DateTimeOffset(Convert.ToDateTime(time)).ToETagString(_userManager.GetCount()));
            }
        }
    }
}