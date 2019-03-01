using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.IServices
{
    public interface IBaseServices<T> where T : class, new()
    {
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> whereLamada);

        IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLamada);
    }
}