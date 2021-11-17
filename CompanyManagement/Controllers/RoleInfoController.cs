using Entity;
using ICompanyBll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utility.ApiResult;

namespace CompanyManagement.Controllers
{
    public class RoleInfoController : Controller
    {
        readonly IRoleInfoBll _iRoleInfoBll;
        readonly IUserInfoBll _iUserInfoBll;
        readonly IR_UserInfo_RoleInfoBll _iR_userInfo_roleInfoBll;
        readonly IRRoleInfoMenuInfoBll _iRRoleInfoMenuInfoBll;
        readonly IMenuInfoBll _iMenuInfoBll;
        public RoleInfoController(IRoleInfoBll iRoleInfoBll, IUserInfoBll iUserInfoBll, IR_UserInfo_RoleInfoBll iR_userInfo_roleInfoBll, IRRoleInfoMenuInfoBll iRRoleInfoMenuInfoBll, IMenuInfoBll iMenuInfoBll)
        {
            _iRoleInfoBll = iRoleInfoBll;
            _iUserInfoBll = iUserInfoBll;
            _iR_userInfo_roleInfoBll = iR_userInfo_roleInfoBll;
            _iRRoleInfoMenuInfoBll = iRRoleInfoMenuInfoBll;
            _iMenuInfoBll = iMenuInfoBll;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Query(string roleName, int page, int limit)
        {
            var roleInfo = await _iRoleInfoBll.Query();
            int count = roleInfo.Count;
            if (!string.IsNullOrEmpty(roleName))
            {
                roleInfo = await _iRoleInfoBll.QueryPage(page, limit, x => x.IsDelete == false && x.RoleName.Contains(roleName));
                count = roleInfo.Count;
            }

            if (string.IsNullOrEmpty(roleName))
            {
                roleInfo = await _iRoleInfoBll.QueryPage(page, limit, x => x.IsDelete == false);
            }
            return Json(ApiResulthelp.Success(roleInfo, count));
        }

        public IActionResult CreateView()
        {
            return View();
        }

        public async Task<IActionResult> Create(string rolename, string desctiption)
        {
            if (string.IsNullOrEmpty(rolename))
            {
                return Json(ApiResulthelp.Error("角色名称不能为空"));
            }
            else if (string.IsNullOrEmpty(desctiption))
            {
                return Json(ApiResulthelp.Error("描述不能为空"));
            }

            if (_iRoleInfoBll.CreateVerification(x => x.RoleName == rolename && x.IsDelete == false))
            {
                return Json(ApiResulthelp.Error("角色名称重复不能为空"));
            }

            RoleInfo roleInfo = new()
            {
                Id = Guid.NewGuid().ToString(),
                RoleName = rolename,
                Description = desctiption,
                CreateTime = DateTime.Now,
                IsDelete = false
            };

            var b = await _iRoleInfoBll.Create(roleInfo);
            if (b) return Json(ApiResulthelp.Success(b));
            return Json(ApiResulthelp.Error("添加失败"));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(List<string> Id)
        {
            int b = await _iRoleInfoBll.FakeDelete(x => Id.Contains(x.Id) && x.IsDelete == false, x => new RoleInfo()
            {
                IsDelete = true,
                DeleteTime = DateTime.Now
            });


            if (b > 0) return Json(ApiResulthelp.Success(b));
            return Json(ApiResulthelp.Error("删除失败"));
        }

        public IActionResult UpdateView()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(string Id, string rolename, string desctiption)
        {
            var b = await _iRoleInfoBll.Update(Id, rolename, desctiption);
            if (b) return Json(ApiResulthelp.Success("修改成功"));
            return Json(ApiResulthelp.Error("修改失败"));
        }

        public IActionResult BindView()
        {
            return View();
        }

        public IActionResult BindMenuView()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BindUserInfo(string roleId, List<string> userinfoId)
        {
            var alluserInfoId = await _iR_userInfo_roleInfoBll.Query(x => x.RoleId == roleId);

            foreach (var item in alluserInfoId)
            {
                if (!userinfoId.Contains(item.UserId))
                {
                    await _iR_userInfo_roleInfoBll.Delete(item.Id);
                }
            }

            List<R_UserInfo_RoleInfo> addEntityList = new List<R_UserInfo_RoleInfo>();

            foreach (var id in userinfoId)
            {
                if (!alluserInfoId.Any(x => x.UserId == id))
                {
                    R_UserInfo_RoleInfo rUserInfoRoleInfo = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreateTime = DateTime.Now,
                        RoleId = roleId,
                        UserId = id
                    };
                    addEntityList.Add(rUserInfoRoleInfo);
                }
            }

            await _iR_userInfo_roleInfoBll.BatchInsert(addEntityList);

            return Json(ApiResulthelp.Success("成功"));
        }

        public async Task<IActionResult> QueryUserInfo(string roleInfoId)
        {
            var userInfo = await _iUserInfoBll.QueryDb().Where(x => x.IsDelete == false).Select(x => new
            {
                x.Id,
                x.UserName
            }).ToListAsync();
            var bindUserInfo = await _iR_userInfo_roleInfoBll.QueryDb().Where(x => x.RoleId == roleInfoId).Select(x => x.UserId).ToListAsync();
            return Json(new
            {
                userInfo,
                bindUserInfo
            });
        }

        public async Task<IActionResult> QueryRoleInfo(string roleInfoId)
        {
            var menuInfo = await _iMenuInfoBll.QueryDb().Where(x => x.IsDelete == false).Select(x => new
            {
                x.Id,
                x.Title
            }).ToArrayAsync();
            var bindMenuInfo = await _iRRoleInfoMenuInfoBll.QueryDb().Where(x => x.RoleId == roleInfoId).Select(x => x.MenuId).ToListAsync();

            return Json(new
            {
                menuInfo,
                bindMenuInfo
            });
        }

        [HttpPost]
        public async Task<IActionResult> BindRoleInfo(string roleId, List<string> menuInfoId)
        {
            var allRoleInfoId = await _iRRoleInfoMenuInfoBll.Query(x => x.RoleId == roleId);


            foreach (var item in allRoleInfoId)
            {
                if (!menuInfoId.Contains(item.RoleId))
                {
                    await _iRRoleInfoMenuInfoBll.Delete(item.Id);
                }
            }

            List<R_RoleInfo_MenuInfo> addEntityList = new List<R_RoleInfo_MenuInfo>();

            foreach (var id in menuInfoId)
            {
                if (!_iRRoleInfoMenuInfoBll.QueryDb().Any(x => x.RoleId == id))
                {
                    R_RoleInfo_MenuInfo rRoleInfoMenuInfo = new R_RoleInfo_MenuInfo();
                    rRoleInfoMenuInfo.Id = Guid.NewGuid().ToString();
                    rRoleInfoMenuInfo.RoleId = roleId;
                    rRoleInfoMenuInfo.MenuId = id;
                    rRoleInfoMenuInfo.CreateTime = DateTime.Now;

                    addEntityList.Add(rRoleInfoMenuInfo);
                }

            }

            await _iRRoleInfoMenuInfoBll.BatchInsert(addEntityList);

            return Json(ApiResulthelp.Success(true));
        }

    }
}
