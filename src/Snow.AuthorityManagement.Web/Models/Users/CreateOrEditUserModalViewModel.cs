using AutoMapper;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Web.Mvc.Models.Users
{
    public class CreateOrEditUserModalViewModel : GetUserForEditOutput
    {
        private IMapper Mapper { get; set; }

        public CreateOrEditUserModalViewModel(GetUserForEditOutput output)
        {
            //Mapper.Map(output, this);
            //output.MapTo(this);
        }

        //public bool CanChangeUserName
        //{
        //    get { return User.UserName != Authorization.Users.User.AdminUserName; }
        //}

        //public int AssignedRoleCount
        //{
        //    get { return Roles.Count(r => r.IsAssigned); }
        //}

        public bool IsEditMode
        {
            get { return User.ID.HasValue; }
        }
    }
}