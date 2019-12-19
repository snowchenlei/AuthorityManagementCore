using System;

namespace Snow.AuthorityManagement.Core.Entities
{
    /// <summary>
    /// 拥有一个int主键
    /// </summary>
    public class Entity : Entity<int>, IEntity<int>
    {
    }

    public class Entity<TPrimaryKey>
    {
        public int ID { get; set; }
        public DateTime? AddTime { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"[{GetType().Name} {ID}]";
        }
    }
}