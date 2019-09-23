using CacheCow.Server;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Web.Core.Common.ETag.User
{
    public class UserETagExtractor : ITimedETagExtractor<GetUserForEditOutput>
    {
        public TimedEntityTagHeaderValue Extract(GetUserForEditOutput viewModel)
        {
            if (viewModel == null)
                return null;
            DateTimeOffset
                lastTimeOffset = new DateTimeOffset(viewModel.User.LastModificationTime ?? DateTime.MinValue);
            return new TimedEntityTagHeaderValue(lastTimeOffset.ToETagString());
        }

        public TimedEntityTagHeaderValue Extract(object viewModel)
        {
            return Extract(viewModel as GetUserForEditOutput);
        }
    }
}