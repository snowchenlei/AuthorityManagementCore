using Microsoft.EntityFrameworkCore;
using Snow.AuthorityManagement.Core.Entities.Authorization;

namespace Snow.AuthorityManagement.Data
{
    public class AuthorityManagementContext : DbContext
    {
        public AuthorityManagementContext(DbContextOptions<AuthorityManagementContext> options) : base(options)
        {
        }

        //权限管理
        public virtual DbSet<User> User { get; set; }

        public virtual DbSet<Role> Role { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ////表名不用复数形式
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            ////移除一对多的级联删除约定，想要级联删除可以在 EntityTypeConfiguration<TEntity>的实现类中进行控制
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            ////多对多启用级联删除约定，不想级联删除可以在删除前判断关联的数据进行拦截
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            //配置联合主键
            //modelBuilder.Entity<UserInfoModuleElement>()
            //  .HasKey(r => new { r.UserInfoID, r.ModuleID, r.ModuleElementID });
            //modelBuilder.Entity<RoleModuleElement>()
            //  .HasKey(r => new { r.RoleID, r.ModuleID, r.ModuleElementID });

            //modelBuilder.Entity<RoleUserInfo>()
            //    .HasKey(r => new { r.RoleID, r.UserInfoID });
            //modelBuilder.Entity<RoleModule>()
            //    .HasKey(r => new { r.RoleID, r.ModuleID });
            //modelBuilder.Entity<ModuleUserInfo>()
            //    .HasKey(r => new { r.ModuleID, r.UserInfoID });
            //modelBuilder.Entity<ModuleElementModule>()
            //   .HasKey(r => new { r.ModuleID, r.ModuleElementID });

            base.OnModelCreating(modelBuilder);
        }
    }
}