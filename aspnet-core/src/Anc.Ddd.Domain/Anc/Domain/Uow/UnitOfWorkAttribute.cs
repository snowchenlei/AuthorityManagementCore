using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Domain.Uow
{
    /// <summary>
    /// 工作单元特性标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public class UnitOfWorkAttribute : Attribute
    {
        public bool IsDisabled { get; set; }
    }
}