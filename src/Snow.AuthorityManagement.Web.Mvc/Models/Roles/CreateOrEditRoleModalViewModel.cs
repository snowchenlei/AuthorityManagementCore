using AutoMapper;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Web.Models.Roles
{
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput
    {
        //private IMapper Mapper { get; set; }

        public CreateOrEditRoleModalViewModel(GetRoleForEditOutput output)
        {
            //Mapper.Map<CreateOrEditRoleModalViewModel>(output);
        }

        public bool IsEditMode
        {
            get { return Role.ID.HasValue; }
        }
    }
}