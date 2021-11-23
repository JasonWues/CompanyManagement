using Entity;
using ICompanyBll;
using Microsoft.AspNetCore.Mvc;
using Utility.ApiResult;

namespace CompanyManagement.Controllers;

public class ConsumableInfoController : Controller
{
    readonly IConsumableInfoBll _iConsumableInfoBll;
    readonly ICategoryBll _iCategoryBll;
    public ConsumableInfoController(IConsumableInfoBll iConsumableInfoBll,ICategoryBll iCategoryBll)
    {
        _iConsumableInfoBll = iConsumableInfoBll;
        _iCategoryBll = iCategoryBll;
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

    public async Task<IActionResult> QueryCategoryName()
    {
        return Json(ApiResulthelp.Success(await _iCategoryBll.Query()));
    }

    public async Task<IActionResult> Create(string description,string name,string categoryId, string specification,string unit,decimal money)
    {
        ConsumableInfo consumableInfo = new ConsumableInfo();
        consumableInfo.Id = Guid.NewGuid().ToString();
        consumableInfo.Description = description;
        consumableInfo.Name = name;
        consumableInfo.CategoryId = categoryId;
        consumableInfo.Specification = specification;
        consumableInfo.Unit = unit;
        consumableInfo.Money = money;
        consumableInfo.CreateTime = DateTime.Now;
        consumableInfo.IsDelete = false;

        if (_iConsumableInfoBll.CreateVerification(x => x.Name == name && x.IsDelete == false))
        {
            return Json(ApiResulthelp.Error("耗材已存在"));
        }

        var b =  await _iConsumableInfoBll.Create(consumableInfo);
        if (b) return Json(ApiResulthelp.Success(b));
        return Json(ApiResulthelp.Error("添加失败"));

    }

    public async Task<IActionResult> Delete(List<string> Id)
    {
        var b = await _iConsumableInfoBll.FakeDelete(x => Id.Contains(x.Id),x => new ConsumableInfo() { IsDelete = true,DeleteTime = DateTime.Now});
        if (b > 0) return Json(ApiResulthelp.Success("删除成功"));
        return Json(ApiResulthelp.Error("失败"));
    }
}