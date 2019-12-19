using Anc.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Snow.AuthorityManagement.Core.Authorization.AuditLogs;
using Snow.AuthorityManagement.Core.Authorization.Menus;
using Snow.AuthorityManagement.Core.Authorization.Permissions;
using Snow.AuthorityManagement.Core.Authorization.Roles;
using Snow.AuthorityManagement.Core.Authorization.UserRoles;
using Snow.AuthorityManagement.Core.Entities.Authorization;

namespace Snow.AuthorityManagement.EntityFrameworkCore
{
    public class AuthorityManagementContext : DbContext
    {
        public AuthorityManagementContext(DbContextOptions<AuthorityManagementContext> options) : base(options)
        {
        }

        //权限管理
        public virtual DbSet<User> User { get; set; }

        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<AncPermission> Permission { get; set; }

        public virtual DbSet<AuditLog> AuditLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ////表名不用复数形式
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            ////移除一对多的级联删除约定，想要级联删除可以在 EntityTypeConfiguration<TEntity>的实现类中进行控制
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            ////多对多启用级联删除约定，不想级联删除可以在删除前判断关联的数据进行拦截
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            //https://docs.microsoft.com/zh-cn/ef/core/modeling/relationships#other-relationship-patterns
            //modelBuilder.Entity<UserRole>()
            //    .HasKey(t => new { t.UserID, t.RoleID });

            modelBuilder.Entity<AncPermission>()
                .ToTable("Permission");
            modelBuilder.Entity<UserRole>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(pt => pt.UserID);

            modelBuilder.Entity<UserRole>()
                .HasOne(pt => pt.Role)
                .WithMany(t => t.UserRoles)
                .HasForeignKey(pt => pt.RoleID);

            base.OnModelCreating(modelBuilder);
        }
    }
}