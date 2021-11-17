using Entity;
using Entity.DTO;
using ICompanyBll;
using Microsoft.AspNetCore.Mvc;
using Utility.ApiResult;

namespace CompanyManagement.Controllers
{
    public class DepartmentInfoController : Controller
    {
        readonly IDepartmentInfoBll _iDepartmentInfoBll;
        readonly IUserInfoBll _iUserInfoBll;

        public DepartmentInfoController(IDepartmentInfoBll iDepartmentInfoBll, IUserInfoBll userInfoBll)
        {
            _iDepartmentInfoBll = iDepartmentInfoBll;
            _iUserInfoBll = userInfoBll;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Query(string departmentName, int page, int limit)
        {
            (List<DepartmentInfo_UserInfo> list, int count) = await _iDepartmentInfoBll.Query(departmentName, page, limit);

            return Json(ApiResulthelp.Success(list, count));
        }

        public IActionResult CreateView()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string departmentName, string leaderId, string parentId, string description)
        {
            if (string.IsNullOrEmpty(departmentName))
            {
                return Json(ApiResulthelp.Error("部门名称不能为空"));
            }
            else if (string.IsNullOrEmpty(leaderId))
            {
                return Json(ApiResulthelp.Error("主管ID不能为空"));
            }

            if (_iDepartmentInfoBll.CreateVerification(x => x.DepartmentName == departmentName && x.IsDelete == false))
            {
                return Json(ApiResulthelp.Error("部门名称重复"));
            }

            DepartmentInfo entity = new()
            {
                Id = Guid.NewGuid().ToString(),
                DepartmentName = departmentName,
                Description = description,
                LeaderId = leaderId,
                ParentId = parentId,
                CreateTime = DateTime.Now,
                IsDelete = false
            };

            var b = await _iDepartmentInfoBll.Create(entity);
            if (b) return Json(ApiResulthelp.Success(b));
            return Json(ApiResulthelp.Error("添加失败"));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(List<string> Id)
        {
            int b = await _iDepartmentInfoBll.FakeDelete(x => Id.Contains(x.Id) && x.IsDelete == false, x => new DepartmentInfo()
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
        public async Task<IActionResult> Update(string Id, string departmentName, string leaderId, string parentId, string description)
        {
            if (Id != parentId)
            {
                var b = await _iDepartmentInfoBll.Update(Id, departmentName, leaderId, parentId, description);
                if (b) return Json(ApiResulthelp.Success(b));
                return Json(ApiResulthelp.Error("修改失败"));
            }
            else
            {
                return Json(ApiResulthelp.Error("父部门不能是本部门"));
            }

        }

        public async Task<IActionResult> QueryDepartmentName(string Id)
        {
            var userinfo = await _iDepartmentInfoBll.Find(Id);

            return Json(ApiResulthelp.Success(userinfo));
        }

        public async Task<IActionResult> QueryDepartmentinfo(string Id)
        {
            var departmentinfo = await _iDepartmentInfoBll.Query(x => x.IsDelete == false && x.Id != Id);
            var userinfo = await _iUserInfoBll.Query(x => x.IsDelete == false);
            var enumerable = userinfo.Select(x => new
            {
                x.Id,
                x.Account
            });
            return Json(new
            {
                enumerable,
                departmentinfo
            });
        }

    }
}
