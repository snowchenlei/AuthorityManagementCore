using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Core.Entities.Authorization
{
    public class Permission
    {
        public int Id { get; set; }
        public bool IsGranted { get; set; }
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }
}