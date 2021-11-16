using ICompanyBll;
using Microsoft.AspNetCore.Mvc;
using Utility.ApiResult;

namespace CompanyManagement.Controllers;

public class ConsumableInfoController : Controller
{
    readonly IConsumableInfoBll _iConsumableInfoBll;
    public ConsumableInfoController(IConsumableInfoBll iConsumableInfoBll)
    {
        _iConsumableInfoBll = iConsumableInfoBll;
    }
    public IActionResult Index()
    {
        return View();
    }
    
    public async Task<IActionResult> Query(string name, int page, int limit)
    {
        var(list,count) = await _iConsumableInfoBll.Query(name, page, limit);
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
    //public IActionResult Create()
    //{

    //}
}