﻿using Entity;
using ICompanyBll;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Utility.ApiResult;

namespace CompanyManagement.Controllers
{
    public class ConsumableRecordController : Controller
    {
        readonly IConsumableRecordBll _iConsumableRecordBll;
        public ConsumableRecordController(IConsumableRecordBll iConsumableRecordBll)
        {
            _iConsumableRecordBll = iConsumableRecordBll;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Query(int page, int limit, string consumableId)
        {
            (var list, var count) = await _iConsumableRecordBll.Query(page, limit, consumableId);
            return Json(ApiResulthelp.Success(list, count));
        }

        public IActionResult CreateView()
        {
            return View();
        }

        public async Task<IActionResult> Create(string consumableId, int num)
        {
            string userInfoJson = HttpContext.Session.GetString("UserInfo");
            UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(userInfoJson);

            if (userInfo == null)
            {
                return Json(ApiResulthelp.Error("没有登入信息"));
            }

            if (string.IsNullOrEmpty(consumableId))
            {
                return Json(ApiResulthelp.Error("耗材不能为空"));
            }
            else if (num == 0)
            {
                return Json(ApiResulthelp.Error("数量错误"));
            }

            ConsumableRecord consumableRecord = new ConsumableRecord
            {
                Id = Guid.NewGuid().ToString(),
                ConsumableId = consumableId,
                Num = num,
                Type = 1,
                CreateTime = DateTime.Now,
                Creator = userInfo.Id
            };

            var b = await _iConsumableRecordBll.Create(consumableRecord);
            if (b) return Json(ApiResulthelp.Success(b));
            return Json(ApiResulthelp.Error("错误"));
        }

        public async Task<IActionResult> UpLoad(List<IFormFile> excelFiles)
        {

        }
    }
}
