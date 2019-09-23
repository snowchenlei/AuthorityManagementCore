using CacheCow.Server;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Web.Core.Common.ETag.Role
{
    public class RoleETagExtractor : ITimedETagExtractor<GetRoleForEditOutput>
    {
        public TimedEntityTagHeaderValue Extract(GetRoleForEditOutput viewModel)
        {
            if (viewModel == null)
                return null;
            DateTimeOffset
                lastTimeOffset = new DateTimeOffset(viewModel.Role.LastModificationTime ?? DateTime.MinValue);
            return new TimedEntityTagHeaderValue(lastTimeOffset.ToETagString());
        }

        public TimedEntityTagHeaderValue Extract(object viewModel)
        {
            return Extract(viewModel as GetRoleForEditOutput);
        }
    }
}