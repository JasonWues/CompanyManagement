using CompanyBll;
using CompanyDal;
using Entity;
using ICompanyBll;
using ICompanyDal;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddDbContext<CompanyContext>();
#region IOC
builder.Services.AddScoped<IUserInfoDal, UserInfoDal>();
builder.Services.AddScoped<IUserInfoBll, UserInfoBll>();
builder.Services.AddScoped<IRoleInfoDal, RoleInfoDal>();
builder.Services.AddScoped<IRoleInfoBll, RoleInfoBll>();
builder.Services.AddScoped<IRUserInfo_RoleInfoDal, RuserInfo_RoleInfoDal>();
builder.Services.AddScoped<IR_UserInfo_RoleInfoBll, R_UserInfo_RoleInfoBll>();
builder.Services.AddScoped<IDepartmentInfoDal, DepartmentInfoDal>();
builder.Services.AddScoped<IDepartmentInfoBll, DepartmentInfoBll>();
builder.Services.AddScoped<IMenuInfoDal, MenuInfoDal>();
builder.Services.AddScoped<IMenuInfoBll, MenuInfoBll>();
builder.Services.AddScoped<IRRoleInfoMenuInfoDal, RRoleInfoMenuInfoDal>();
builder.Services.AddScoped<IRRoleInfoMenuInfoBll, RRoleInfoMenuInfoBll>();
builder.Services.AddScoped<IConsumableInfoDal, ConsumableInfoDal>();
builder.Services.AddScoped<IConsumableInfoBll, ConsumableInfoBll>();
builder.Services.AddScoped<ICategoryDal, CategoryDal>();
builder.Services.AddScoped<ICategoryBll, CategoryBll>();
builder.Services.AddScoped<IConsumableRecordDal, ConsumableRecordDal>();
builder.Services.AddScoped<IConsumableRecordBll, ConsumableRecordBll>();
builder.Services.AddScoped<IWorkFlow_InstanceDal, WorkFlow_InstanceDal>();
builder.Services.AddScoped<IWorkFlow_InstanceBll, WorkFlow_InstanceBll>();
builder.Services.AddScoped<IWorkFlow_InstanceStepDal, WorkFlow_InstanceStepDal>();
builder.Services.AddScoped<IWorkFlow_InstanceStepBll, WorkFlow_InstanceStepBll>();
builder.Services.AddScoped<IWorkFlow_ModelDal, WorkFlow_ModelDal>();
builder.Services.AddScoped<IWorkFlow_ModelBll, WorkFlow_ModelBll>();
#endregion

//InitDB();

static string MD5Encrypt16(string password)
{
    var md5 = new MD5CryptoServiceProvider();
    string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(password)), 4, 8);
    t2 = t2.Replace("-", "");
    return t2;
}

static void InitDB()
{
    //var contextOptions = new DbContextOptionsBuilder<CompanyContext>().UseSqlServer("server=.;database=CompanySystem;uid=sa;pwd=123456;").Options;
    var contextOptions = new DbContextOptionsBuilder<CompanyContext>().UseSqlServer(@"server=DESKTOP-QOGKNNM\SQLEXPRESS;database=CompanySystem;uid=sa;pwd=123456;").Options;
    using (CompanyContext context = new(contextOptions))
    {
        context.Database.EnsureDeleted();
        //如果没有数据库则创建数据库
        context.Database.EnsureCreated();

        var userInfo = new UserInfo()
        {
            Id = Guid.NewGuid().ToString(),
            Account = "admin",
            UserName = "管理员",
            Sex = 1,
            PassWord = MD5Encrypt16("123456"),
            CreateTime = DateTime.Now,
            IsAdmin = 1
        };

        context.UserInfo.AddRange(userInfo, new UserInfo()
        {
            Id = Guid.NewGuid().ToString(),
            Account = "zhangsan",
            UserName = "张三",
            Sex = 1,
            PassWord = MD5Encrypt16("123456"),
            CreateTime = DateTime.Now
        }, new UserInfo()
        {
            Id = Guid.NewGuid().ToString(),
            Account = "lisi",
            UserName = "李四",
            Sex = 0,
            PassWord = MD5Encrypt16("123456"),
            CreateTime = DateTime.Now
        });

        #region 初始化部门相关信息
        context.DepartmentInfo.AddRange(new DepartmentInfo()
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            DepartmentName = "人事部",
            Description = "我是人事部"
        }, new DepartmentInfo()
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            DepartmentName = "财务部",
            Description = "我是财务部"
        });
        #endregion

        #region 初始化角色相关信息
        var roleInfo = new RoleInfo()
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            RoleName = "系统管理员",
            Description = "系统管理员"
        };
        context.RoleInfo.AddRange(roleInfo);

        //添加管理员为系统管理员角色
        context.RUserInfoRoleInfo.Add(new R_UserInfo_RoleInfo()
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            RoleId = roleInfo.Id,
            UserId = userInfo.Id
        });
        #endregion

        #region  初始化菜单相关信息
        var parentMenu = new MenuInfo()
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            Title = "系统管理",
            Icon = "fa fa-window-maximize",
            Target = "_self",
            Level = 0,
            Sort = 1000
        };

        context.MenuInfo.AddRange(parentMenu, new MenuInfo()
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            Title = "用户管理",
            Href = "/UserInfo/Index",
            Icon = "fa fa-window-maximize",
            Target = "_self",
            Level = 1,
            Sort = 1001,
            ParentId = parentMenu.Id
        }, new MenuInfo()
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            Title = "角色管理",
            Href = "/RoleInfo/Index",
            Icon = "fa fa-window-maximize",
            Target = "_self",
            Level = 1,
            Sort = 1002,
            ParentId = parentMenu.Id
        }, new MenuInfo()
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            Title = "部门管理",
            Href = "/DepartmentInfo/Index",
            Icon = "fa fa-window-maximize",
            Target = "_self",
            Level = 1,
            Sort = 1003,
            ParentId = parentMenu.Id
        }, new MenuInfo()
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            Title = "菜单管理",
            Href = "/MenuInfo/Index",
            Icon = "fa fa-window-maximize",
            Target = "_self",
            Level = 1,
            Sort = 1004,
            ParentId = parentMenu.Id
        }, new MenuInfo()
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            Title = "耗材管理",
            Href = "/ConsumableInfo/Index",
            Icon = "fa fa-window-maximize",
            Target = "_self",
            Level = 1,
            Sort = 1005,
            ParentId = parentMenu.Id
        }, new MenuInfo()
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            Title = "物资类别",
            Href = "/Category/Index",
            Icon = "fa fa-window-maximize",
            Target = "_self",
            Level = 1,
            Sort = 1006,
            ParentId = parentMenu.Id
        }, new MenuInfo()
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            Title = "耗材入库",
            Href = "/ConsumableRecord/Index",
            Icon = "fa fa-window-maximize",
            Target = "_self",
            Level = 0,
            Sort = 21002,
            ParentId = parentMenu.Id
        }, new MenuInfo()
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            Title = "工作流模板",
            Href = "/WorkFlowModel/Index",
            Icon = "fa fa-window-maximize",
            Target = "_self",
            Level = 0,
            Sort = 31000,
            ParentId = parentMenu.Id
        }, new MenuInfo()
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            Title = "申请流程",
            Href = "/WorkFlowInstance/Index",
            Icon = "fa fa-window-maximize",
            Target = "_self",
            Level = 0,
            Sort = 31001,
            ParentId = parentMenu.Id
        }, new MenuInfo()
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            Title = "流程审批",
            Href = "/WorkFlowInstanceStep/Index",
            Icon = "fa fa-window-maximize",
            Target = "_self",
            Level = 0,
            Sort = 31002,
            ParentId = parentMenu.Id
        });
        #endregion

        #region 初始化耗材相关信息
        context.ConsumableInfo.AddRange(new ConsumableInfo
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            Name = "铅笔",
            WarningNum = 10,
            Money = 1,
            Unit = "支",
            Specification = "HB",
            Description = "我是2B铅笔"
        }, new ConsumableInfo
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            Name = "抽纸",
            WarningNum = 10,
            Money = 2,
            Unit = "包",
            Specification = "200抽",
            Description = "我是抽纸"
        }, new ConsumableInfo
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            Name = "黑色水性笔",
            WarningNum = 10,
            Money = 1,
            Unit = "支",
            Specification = "笔盖式",
            Description = "我是黑色水性笔"
        }, new ConsumableInfo
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            Name = "软抄笔记本",
            WarningNum = 10,
            Money = 3,
            Unit = "本",
            Specification = "100页",
            Description = "我是软抄笔记本"
        });
        #endregion

        #region 初始化类别相关信息
        context.Category.AddRange(new Category()
        {
            Id = Guid.NewGuid().ToString(),
            CategoryName = "日常用品"
        },
        new Category()
        {
            Id = Guid.NewGuid().ToString(),
            CategoryName = "办公用品"
        });
        #endregion

        context.WorkFlow_Models.AddRange(new WorkFlow_Model()
        {
            Id = Guid.NewGuid().ToString(),
            Title = "申请耗材",
            Description = "申请耗材流程",
            CreateTime = DateTime.Now
        });

        context.SaveChanges();
    }
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();