using System;

namespace Snow.AuthorityManagement.Core.Dto
{
    public class EntityDto : EntityDto<int> { }

    public class EntityDto<TPrimaryKey>
    {
        public int ID { get; set; }
        public DateTime AddTime { get; set; }
    }
}