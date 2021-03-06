using CompanyBll;
using CompanyDal;
using Entity;
using ICompanyBll;
using ICompanyDal;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

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
        //??????????????????????????
        context.Database.EnsureCreated();

        var userInfo = new UserInfo()
        {
            Id = Guid.NewGuid().ToString(),
            Account = "admin",
            UserName = "??????",
            Sex = 1,
            PassWord = MD5Encrypt16("123456"),
            CreateTime = DateTime.Now,
            IsAdmin = 1
        };

        context.UserInfo.AddRange(userInfo, new UserInfo()
        {
            Id = Guid.NewGuid().ToString(),
            Account = "zhangsan",
            UserName = "????",
            Sex = 1,
            PassWord = MD5Encrypt16("123456"),
            CreateTime = DateTime.Now
        }, new UserInfo()
        {
            Id = Guid.NewGuid().ToString(),
            Account = "lisi",
            UserName = "????",
            Sex = 0,
            PassWord = MD5Encrypt16("123456"),
            CreateTime = DateTime.Now
        });

        #region ??????????????????
        context.DepartmentInfo.AddRange(new DepartmentInfo()
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            DepartmentName = "??????",
            Description = "??????????"
        }, new DepartmentInfo()
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            DepartmentName = "??????",
            Description = "??????????"
        });
        #endregion

        #region ??????????????????
        var roleInfo = new RoleInfo()
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            RoleName = "??????????",
            Description = "??????????"
        };
        context.RoleInfo.AddRange(roleInfo);

        //??????????????????????????
        context.RUserInfoRoleInfo.Add(new R_UserInfo_RoleInfo()
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            RoleId = roleInfo.Id,
            UserId = userInfo.Id
        });
        #endregion

        #region  ??????????????????
        var parentMenu = new MenuInfo()
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            Title = "????????",
            Icon = "fa fa-window-maximize",
            Target = "_self",
            Level = 0,
            Sort = 1000
        };

        context.MenuInfo.AddRange(parentMenu, new MenuInfo()
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            Title = "????????",
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
            Title = "????????",
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
            Title = "????????",
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
            Title = "????????",
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
            Title = "????????",
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
            Title = "????????",
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
            Title = "????????",
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
            Title = "??????????",
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
            Title = "????????",
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
            Title = "????????",
            Href = "/WorkFlowInstanceStep/Index",
            Icon = "fa fa-window-maximize",
            Target = "_self",
            Level = 0,
            Sort = 31002,
            ParentId = parentMenu.Id
        });
        #endregion

        #region ??????????????????
        context.ConsumableInfo.AddRange(new ConsumableInfo
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            Name = "????",
            WarningNum = 10,
            Money = 1,
            Unit = "??",
            Specification = "HB",
            Description = "????2B????"
        }, new ConsumableInfo
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            Name = "????",
            WarningNum = 10,
            Money = 2,
            Unit = "??",
            Specification = "200??",
            Description = "????????"
        }, new ConsumableInfo
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            Name = "??????????",
            WarningNum = 10,
            Money = 1,
            Unit = "??",
            Specification = "??????",
            Description = "??????????????"
        }, new ConsumableInfo
        {
            Id = Guid.NewGuid().ToString(),
            CreateTime = DateTime.Now,
            Name = "??????????",
            WarningNum = 10,
            Money = 3,
            Unit = "??",
            Specification = "100??",
            Description = "??????????????"
        });
        #endregion

        #region ??????????????????
        context.Category.AddRange(new Category()
        {
            Id = Guid.NewGuid().ToString(),
            CategoryName = "????????"
        },
        new Category()
        {
            Id = Guid.NewGuid().ToString(),
            CategoryName = "????????"
        });
        #endregion

        context.WorkFlow_Models.AddRange(new WorkFlow_Model()
        {
            Id = Guid.NewGuid().ToString(),
            Title = "????????",
            Description = "????????????",
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