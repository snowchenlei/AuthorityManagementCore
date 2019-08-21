using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Anc.Domain.Entities
{
    [Serializable]
    public class EntityNotFoundException : AncException
    {
        /// <summary>
        /// 实体类型。
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// 实体ID。
        /// </summary>
        public object ID { get; set; }

        /// <summary>
        /// 新建一个 <see cref="EntityNotFoundException"/> 对象。
        /// </summary>
        public EntityNotFoundException()
        {
        }

        /// <summary>
        /// 新建一个 <see cref="EntityNotFoundException"/> 对象。
        /// </summary>
        public EntityNotFoundException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        /// <summary>
        /// 新建一个 <see cref="EntityNotFoundException"/> 对象。
        /// </summary>
        public EntityNotFoundException(Type entityType, object id)
            : this(entityType, id, null)
        {
        }

        /// <summary>
        /// 新建一个 <see cref="EntityNotFoundException"/> 对象。
        /// </summary>
        public EntityNotFoundException(Type entityType, object id, Exception innerException)
            : base($"There is no such an entity. Entity type: {entityType.FullName}, id: {id}", innerException)
        {
            EntityType = entityType;
            ID = id;
        }

        /// <summary>
        /// 新建一个 <see cref="EntityNotFoundException"/> 对象。
        /// </summary>
        /// <param name="message">Exception message</param>
        public EntityNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 新建一个 <see cref="EntityNotFoundException"/> 对象。
        /// </summary>
        /// <param name="message">异常信息</param>
        /// <param name="innerException">内部异常</param>
        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}