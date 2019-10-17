using Anc.DependencyInjection;

namespace Anc.Authorization
{
    public interface IPermission : ITransientDependency
    {
        string Name { get; set; }
    }
}