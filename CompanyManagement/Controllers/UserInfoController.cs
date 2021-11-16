using CompanyManagement.Filter;
using Entity;
using ICompanyBll;
using Microsoft.AspNetCore.Mvc;
using Utility;
using Utility.ApiResult;

namespace CompanyManagement.Controllers
{
    public class UserInfoController : Controller
    {
        readonly IUserInfoBll _userInfoBll;
        readonly IDepartmentInfoBll _departmentInfoBll;
        public UserInfoController(IUserInfoBll userInfoBll, IDepartmentInfoBll departmentInfoBll)
        {
            _userInfoBll = userInfoBll;
            _departmentInfoBll = departmentInfoBll;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Query(string userName, string phoneNum, int page, int limit)
        {
            var (list, count) = await _userInfoBll.Query(userName, phoneNum, page, limit);
            return Json(ApiResulthelp.Success(list, count));
        }

        public IActionResult CreateView()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string account, string userName, string phoneNum, string email, int sex,int isAdmin,string passWord, string departmentInfoId)
        {
            if (string.IsNullOrEmpty(account))
            {
                return Json(ApiResulthelp.Error("账号不能为空"));
            }
            else if (string.IsNullOrEmpty(passWord))
            {
                return Json(ApiResulthelp.Error("密码不能为空"));
            }
            else if (string.IsNullOrEmpty(userName))
            {
                return Json(ApiResulthelp.Error("用户名不能为空"));
            }

            if (_userInfoBll.CreateVerification(x => x.Account == account && x.IsDelete == false))
            {
                return Json(ApiResulthelp.Error("账号重复"));
            }

            UserInfo entity = new()
            {
                Id = Guid.NewGuid().ToString(),
                Account = account,
                UserName = userName,
                PhoneNum = phoneNum,
                Email = email,
                Sex = sex,
                PassWord = MD5.MD5Encrypt16(passWord),
                DepartmentId = departmentInfoId,
                CreateTime = DateTime.Now,
                IsDelete = false,
                isAdmin = isAdmin
            };

            var b = await _userInfoBll.Create(entity);
            if (b) return Json(ApiResulthelp.Success("成功"));
            return Json(ApiResulthelp.Error("添加失败"));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(List<string> Id)
        {
            int b = 0;
            foreach (string id in Id)
            {
                b += await _userInfoBll.FakeDelete(x => x.Id == id && x.IsDelete == false, x => new UserInfo() { IsDelete = true, DeleteTime = DateTime.Now });
            }
            if (b > 0) return Json(ApiResulthelp.Success("成功"));
            return Json(ApiResulthelp.Error("删除失败"));
        }

        public IActionResult UpdateView()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(string Id, string userName, int sex, string phoneNum, string departmentInfoId)
        {
            var b = await _userInfoBll.Update(Id, userName, sex, phoneNum, departmentInfoId);
            if (b) return Json(ApiResulthelp.Success("成功"));
            return Json(ApiResulthelp.Error("修改失败"));
        }

        public async Task<IActionResult> QueryDepartmenName()
        {
            var departmentInfo = await _departmentInfoBll.Query(x => x.IsDelete == false);
            var enumerable = departmentInfo.Select(x => new
            {
                x.Id,
                x.DepartmentName
            });

            return Json(ApiResulthelp.Success(enumerable));
        }

        public async Task<IActionResult> QueryUserInfo(string Id)
        {
            var userinfo = await _userInfoBll.Find(Id);
            var departmentInfo = await _departmentInfoBll.Query(x => x.IsDelete == false);
            var enumerable = departmentInfo.Select(x => new
            {
                x.Id,
                x.DepartmentName
            });
            return Json(new
            {
                userinfo,
                enumerable
            });
        }
    }
}
