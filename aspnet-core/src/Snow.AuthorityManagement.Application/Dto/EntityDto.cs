using System;

namespace Snow.AuthorityManagement.Application.Dto
{
    public class EntityDto : EntityDto<int> { }

    public class EntityDto<TPrimaryKey>
    {
        public int ID { get; set; }
        public DateTime AddTime { get; set; }
    }
}