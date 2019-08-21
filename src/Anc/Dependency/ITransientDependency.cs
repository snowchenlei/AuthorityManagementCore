using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Dependency
{
    /// <summary>
    /// 所有实现此接口的类都自动注册到依赖项注入容器作为单次请求对象。
    /// </summary>
    public interface ITransientDependency
    {
    }
}