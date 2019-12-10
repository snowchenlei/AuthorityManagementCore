using System;

namespace Snow.AuthorityManagement.Core.Entities
{
    internal interface IEntity : IEntity<int>
    {
    }

    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey ID { get; set; }
        DateTime? AddTime { get; set; }
    }
}