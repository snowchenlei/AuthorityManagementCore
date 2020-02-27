using Anc.DependencyInjection;

namespace Anc.Application.Navigation
{
    public abstract class NavigationProvider : ITransientDependency
    {
        /// <summary>
        /// Used to set navigation.
        /// </summary>
        /// <param name="context">Navigation context</param>
        public abstract MenuDefinition GetNavigation();
    }
}