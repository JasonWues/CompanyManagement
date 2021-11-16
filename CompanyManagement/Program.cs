using CompanyBll;
using CompanyDal;
using Entity;
using ICompanyBll;
using ICompanyDal;

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
#endregion


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