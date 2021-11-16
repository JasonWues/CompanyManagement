using ICompanyBll;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagement.Controllers
{
    public class RUserInfoRoleInfoController : Controller
    {
        readonly IR_UserInfo_RoleInfoBll _iRUserInfoRoleInfoBll;
        public RUserInfoRoleInfoController(IR_UserInfo_RoleInfoBll iRUserInfoRoleInfoBll)
        {
            _iRUserInfoRoleInfoBll = iRUserInfoRoleInfoBll;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _iRUserInfoRoleInfoBll.Query());
        }
    }
}
