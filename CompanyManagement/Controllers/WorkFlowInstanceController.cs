using Entity;
using ICompanyBll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Utility.ApiResult;

namespace CompanyManagement.Controllers
{
    public class WorkFlowInstanceController : Controller
    {
        readonly IWorkFlow_InstanceBll _iWorkFlow_InstanceBll;
        readonly IWorkFlow_ModelBll _iWorkFlow_ModelBll;
        readonly IConsumableInfoBll _iConsumableInfoBll;
        public WorkFlowInstanceController(IWorkFlow_InstanceBll iWorkFlow_InstanceBll, IWorkFlow_ModelBll iWorkFlow_ModelBll, IConsumableInfoBll iConsumableInfoBll)
        {
            _iWorkFlow_InstanceBll = iWorkFlow_InstanceBll;
            _iWorkFlow_ModelBll = iWorkFlow_ModelBll;
            _iConsumableInfoBll = iConsumableInfoBll;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateView()
        {
            return View();
        }

        public async Task<IActionResult> Query(int page, int limit, int status)
        {
            (var list, var count) = await _iWorkFlow_InstanceBll.Query(page
                , limit, status);

            return Json(ApiResulthelp.Success(list, count));
        }

        public async Task<IActionResult> Create(string modelId, string outGoodsId, int num, string description, string reason)
        {
            var jsonUserInfo = HttpContext.Session.GetString("UserInfo");
            UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(jsonUserInfo);
            if (userInfo == null)
            {             
                return Json(ApiResulthelp.Error("无登入信息"));
            }
            if (string.IsNullOrEmpty(modelId))
            {
                return Json(ApiResulthelp.Error("流程不能为空"));
            }
            if (string.IsNullOrEmpty(outGoodsId))
            {
                return Json(ApiResulthelp.Error("物品不能为空"));
            }
            if (string.IsNullOrEmpty(userInfo.DepartmentId))
            {
                return Json(ApiResulthelp.Error("当前用户没加入任何部门"));
            }

            WorkFlow_Instance workFlow_Instance = new WorkFlow_Instance()
            {
                Id = Guid.NewGuid().ToString(),
                ModelId = modelId,
                OutGoodsId = outGoodsId,
                OutNum = num,
                Description = description,
                Reason = reason,
                CreateTime = DateTime.Now,
            };

            var b = await _iWorkFlow_InstanceBll.Create(workFlow_Instance);
            if (b) return Json(ApiResulthelp.Success(b));
            return Json(ApiResulthelp.Error("错误"));
        }

        public async Task<IActionResult> QuerySelectOption()
        {
            var workflowmodel = await _iWorkFlow_ModelBll.QueryDb().Select(x => new
            {
                x.Id,
                x.Title
            }).ToListAsync();

            var consumableInfo = await _iConsumableInfoBll.QueryDb().Select(x => new
            {
                x.Id,
                x.Name
            }).ToListAsync();

            return Json(new
            {
                workflowmodel,
                consumableInfo
            });
        }
    }
}
