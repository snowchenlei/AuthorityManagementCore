using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Core.Dto.Permission
{
    public class FlatPermissionDto
    {
        public string ParentName { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool IsGranted { get; set; }

        public ICollection<FlatPermissionDto> Children { get; set; }
    }
}