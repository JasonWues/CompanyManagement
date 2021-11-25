using Entity;
using ICompanyBll;
using Microsoft.AspNetCore.Mvc;
using Utility.ApiResult;

namespace CompanyManagement.Controllers
{
    public class WorkFlowInstanceController : Controller
    {
        readonly IWorkFlow_InstanceBll _iWorkFlow_InstanceBll;
        public WorkFlowInstanceController(IWorkFlow_InstanceBll iWorkFlow_InstanceBll)
        {
            _iWorkFlow_InstanceBll = iWorkFlow_InstanceBll;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateView()
        {
            return View();
        }

        public async Task<IActionResult> Create(string modelId, string outGoodsId, int num, string description, string reason)
        {
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
    }
}
