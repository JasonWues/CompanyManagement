using Microsoft.EntityFrameworkCore;

namespace Entity
{
    public class CompanyContext : DbContext
    {

        public CompanyContext(DbContextOptions<CompanyContext> options) : base(options)
        {

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"server=DESKTOP-QOGKNNM\SQLEXPRESS;database=CompanySystem;uid=sa;pwd=123456;");
            optionsBuilder.UseSqlServer("server=.;database=CompanySystem;uid=sa;pwd=123456;");
        }

        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<RoleInfo> RoleInfo { get; set; }
        public DbSet<R_UserInfo_RoleInfo> RUserInfoRoleInfo { get; set; }
        public DbSet<DepartmentInfo> DepartmentInfo { get; set; } 
        public DbSet<MenuInfo> MenuInfo { get; set; }
        public DbSet<R_RoleInfo_MenuInfo> RRoleInfoMenuInfo { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ConsumableInfo> ConsumableInfo { get; set; }
        public DbSet<ConsumableRecord> ConsumableRecord { get; set; }
    }
}
