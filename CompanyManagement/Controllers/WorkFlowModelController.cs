using Entity;
using ICompanyBll;
using Microsoft.AspNetCore.Mvc;
using Utility.ApiResult;

namespace CompanyManagement.Controllers
{
    public class WorkFlowModelController : Controller
    {
        readonly IWorkFlow_ModelBll _iWorkFlow_ModelBll;
        public WorkFlowModelController(IWorkFlow_ModelBll iWorkFlow_ModelBll)
        {
            _iWorkFlow_ModelBll = iWorkFlow_ModelBll;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Query(string title, int page, int limit)
        {
            if (title == string.Empty)
            {
                var WorkFlowModel = await _iWorkFlow_ModelBll.QueryPage(page, limit);
                return Json(ApiResulthelp.Success(WorkFlowModel, WorkFlowModel.Count));
            }
            else
            {
                var WorkFlowModel = await _iWorkFlow_ModelBll.QueryPage(page, limit, x => x.Title.Contains(title));
                return Json(ApiResulthelp.Success(WorkFlowModel, WorkFlowModel.Count));
            }

        }

        public IActionResult CreateView()
        {
            return View();
        }

        public IActionResult UpdateView()
        {
            return View();
        }

        public async Task<IActionResult> Create(string title, string description)
        {
            if (title == null)
            {
                return Json(ApiResulthelp.Error("标题不能为空"));
            }
            WorkFlow_Model workFlow_Model = new WorkFlow_Model()
            {
                Id = Guid.NewGuid().ToString(),
                Title = title,
                Description = description,
                CreateTime = DateTime.Now,
                isDelete = false
            };

            var b = await _iWorkFlow_ModelBll.Create(workFlow_Model);
            return Json(ApiResulthelp.Success(b));
        }

        public async Task<IActionResult> Update(string Id, string title, string description)
        {
            var b = await _iWorkFlow_ModelBll.Update(Id, title, description);
            if (b) return Json(ApiResulthelp.Success(b));
            return Json(ApiResulthelp.Error("修改失败"));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(List<string> Id)
        {
            int b = await _iWorkFlow_ModelBll.FakeDelete(x => Id.Contains(x.Id) && x.isDelete == false, x => new WorkFlow_Model() { isDelete = true, DeleteTime = DateTime.Now });
            if (b > 0) return Json(ApiResulthelp.Success(b));
            return Json(ApiResulthelp.Error("删除失败"));
        }

        public async Task<IActionResult> QueryWorkFlowModelInfo(string Id)
        {
            var WorkFlowModel = await _iWorkFlow_ModelBll.Query(x => x.Id == Id && x.isDelete == false);
            return Json(ApiResulthelp.Success(WorkFlowModel));
        }

    }
}
