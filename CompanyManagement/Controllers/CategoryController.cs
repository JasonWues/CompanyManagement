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
            (var list,var count) = await _iCategoryBll.Query(categoryName, page, limit);
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

        [HttpPost]
        public async Task<IActionResult> Create(string categoryName, string description)
        {
            Category category = new Category();
            category.Id = Guid.NewGuid().ToString();
            category.CategoryName = categoryName;
            category.Description = description;

            var b = await _iCategoryBll.Create(category);
            if (b) return Json(ApiResulthelp.Success("成功"));
            return Json(ApiResulthelp.Error("失败"));
        }
    }
}
