using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Domain.Entities
{
    public class AncPermission : Entity<Guid>
    {
        public string Name { get; set; }

        public string ProviderName { get; set; }

        public string ProviderKey { get; set; }
        public DateTime CreationTime { get; set; }
    }
}