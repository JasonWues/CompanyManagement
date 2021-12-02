using Entity;
using ICompanyBll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Utility.ApiResult;

namespace CompanyManagement.Controllers
{
    public class ConsumableRecordController : Controller
    {
        readonly IConsumableRecordBll _iConsumableRecordBll;
        readonly IConsumableInfoBll _iConsumableInfoBll;
        public ConsumableRecordController(IConsumableRecordBll iConsumableRecordBll, IConsumableInfoBll iConsumableInfoBll)
        {
            _iConsumableRecordBll = iConsumableRecordBll;
            _iConsumableInfoBll = iConsumableInfoBll;
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

        public async Task<IActionResult> QueryConsumableName()
        {
            var consumableInfo = await _iConsumableInfoBll.QueryDb().AsNoTracking().Where(x => x.IsDelete == false).Select(x => new
            {
                x.Name,
                x.Id
            }).ToListAsync();
            return Json(ApiResulthelp.Success(consumableInfo));
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

        public async Task<IActionResult> UpLoad(IFormFile excelFiles)
        {
            var userInfoJson = HttpContext.Session.GetString("UserInfo");
            UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(userInfoJson);
            var stream = excelFiles.OpenReadStream();
            (var isAdd, string msg) = await _iConsumableRecordBll.UpLoad(stream, userInfo.Id);
            if (isAdd) return Json(ApiResulthelp.Success(isAdd));
            return Json(ApiResulthelp.Error(msg));
        }

        public async Task<IActionResult> DownLoad()
        {
            var stream = await _iConsumableRecordBll.DownLoad();
            return File(stream, "application/octet-stream", "output.xlsx");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(List<string> Id)
        {
            int x = 0;
            foreach (var id in Id)
            {
                x += await _iConsumableRecordBll.Delete(id) ? 1 : 0;
            }
            if (x > 0) return Json(ApiResulthelp.Success(true));
            return Json(ApiResulthelp.Error("删除失败"));
        }
    }
}
