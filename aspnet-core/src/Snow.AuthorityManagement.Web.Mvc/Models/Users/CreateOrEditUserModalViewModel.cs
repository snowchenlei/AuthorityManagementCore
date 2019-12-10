using AutoMapper;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Web.Mvc.Models.Users
{
    public class CreateOrEditUserModalViewModel
    {
        public CreateOrEditUserModalViewModel()
        {
            Roles = new List<RoleSelectDto>();
        }

        public int Id { get; set; }
        public UserEditDto User { get; set; }
        public IEnumerable<RoleSelectDto> Roles { get; set; }

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