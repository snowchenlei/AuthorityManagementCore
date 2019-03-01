using Microsoft.EntityFrameworkCore;

namespace Snow.AuthorityManagement.IRepository
{
    /// <summary>
    /// 业务层调用数据会话层的接口
    /// </summary>
    public interface IDBSession
    {
        DbContext Db { get; }
        bool SaveChanges();
    }
}
