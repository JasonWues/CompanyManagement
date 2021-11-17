using Entity;
using ICompanyBll;
using Microsoft.AspNetCore.Mvc;
using Utility.ApiResult;

namespace CompanyManagement.Controllers
{
    public class CategoryController : Controller
    {
        readonly ICategoryBll _iCategoryBll;
        public CategoryController(ICategoryBll iCategoryBll)
        {
            _iCategoryBll = iCategoryBll;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Query(string categoryName, int page, int limit)
        {
            (var list, var count) = await _iCategoryBll.Query(categoryName, page, limit);
            return Json(ApiResulthelp.Success(list, count));
        }

        public IActionResult CreateView()
        {
            return View();
        }
        public IActionResult UpdateView()
        {
            return View();
        }

        public async Task<IActionResult> Update(string Id, string categoryName, string description)
        {
            var b = await _iCategoryBll.Update(Id, categoryName, description);
            if (b) return Json(ApiResulthelp.Success(b));
            return Json(ApiResulthelp.Error("失败"));
        }

        public async Task<IActionResult> QueryCategoryInfo(string Id)
        {
            var catgory = await _iCategoryBll.Query(x => x.Id == Id);
            return Json(ApiResulthelp.Success(catgory));
        }

        [HttpPost]
        public async Task<IActionResult> Create(string categoryName, string description)
        {
            Category category = new Category();
            category.Id = Guid.NewGuid().ToString();
            category.CategoryName = categoryName;
            category.Description = description;

            var b = await _iCategoryBll.Create(category);
            if (b) return Json(ApiResulthelp.Success(b));
            return Json(ApiResulthelp.Error("失败"));
        }

        //public async Task<IActionResult> Delete(List<string> Id)
        //{

        //    await _iCategoryBll.Delete()
        //}
    }
}
