using Snow.AuthorityManagement.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Snow.AuthorityManagement.RepositoryFactory
{
    public class DBSession:IDBSession
    {
        public DbContext Db { get { return DBContextFactory.CreateDbContext(); } }

        /// <summary>
        /// 一个业务中经常涉及对多张表的操作；我们希望连接一次数据库完成这个操作，提高性能。
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            return Db.SaveChanges() > 0;
        }
    }
}
