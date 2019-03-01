using Snow.AuthorityManagement.IRepository;

namespace Snow.AuthorityManagement.RepositoryFactory
{
    public class DBSessionFactory
    {
        public static IDBSession CreateDBSession()
        {
            //IDBSession dbSession = (IDBSession)CallContext.GetData("dbSession");
            //if (dbSession == null)
            //{
            //    dbSession = new DBSession();
            //    CallContext.SetData("dbSession", dbSession);
            //}
            return new DBSession();
        }
    }
}
