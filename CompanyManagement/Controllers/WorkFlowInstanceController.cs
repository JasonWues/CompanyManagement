using Entity;
using ICompanyBll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> QueryWorkFlowModel()
        {
            var WorkFlowModel = await _iWorkFlow_ModelBll.QueryDb().Select(x => new
            {
                x.Id,
                x.Title
            }).ToListAsync();

            return Json(ApiResulthelp.Success(WorkFlowModel));
        }
    }
}
