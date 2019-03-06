using Snow.AuthorityManagement.Core.Entities.Authorization;
using Snow.AuthorityManagement.Data;
using Snow.AuthorityManagement.IRepository;
using Snow.AuthorityManagement.IRepository.Authorization;

namespace Snow.AuthorityManagement.Repository.Authorization
{
    public partial class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AuthorityManagementContext context) : base(context)
        { }
    }
}