using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Domain.Entities
{
    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey ID { get; set; }

        /// <summary>
        /// 检查是否临时对象。 <see cref="ID"/>
        /// </summary>
        /// <returns>True, 是临时对象</returns>
        bool IsTransient();
    }
}