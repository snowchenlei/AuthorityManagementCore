using Anc.Domain.Entities;
using Anc.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Snow.AuthorityManagement.Core.Authorization.Menus
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class Menu : Entity<int>, IHasModificationTime, IHasCreationTime
    {
        public string Name { get; set; }

        public string PermissionName { get; set; }

        public string Icon { get; set; }

        public string Route { get; set; }

        public int Sort { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public int? ParentID { get; set; }

        public virtual Menu Parent { get; set; }
    }
}