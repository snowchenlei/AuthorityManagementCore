using Anc.Authorization;
using Anc.Domain.Entities;
using Snow.AuthorityManagement.Core.Authorization.Roles;
using Snow.AuthorityManagement.Core.Entities.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Core.Authorization.Permissions
{
    public class Permission : Entity, IPermission
    {
        public bool IsGranted { get; set; }
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }
}