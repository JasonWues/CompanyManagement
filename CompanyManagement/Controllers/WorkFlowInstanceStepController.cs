using Entity;
using ICompanyBll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Utility.ApiResult;

namespace CompanyManagement.Controllers
{
    public class WorkFlowInstanceStepController : Controller
    {
        readonly IWorkFlow_InstanceStepBll _iWorkFlow_InstanceStepBll;
        public WorkFlowInstanceStepController(IWorkFlow_InstanceStepBll iWorkFlow_InstanceStepBll)
        {
            _iWorkFlow_InstanceStepBll = iWorkFlow_InstanceStepBll;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult ReviewView()
        {
            return View();
        }

        public async Task<IActionResult> Query(int page, int limit, int reviewStatus)
        {
            string jsonUserInfo = HttpContext.Session.GetString("UserInfo");
            UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(jsonUserInfo);

            if(userInfo == null)
            {
                return Json(ApiResulthelp.Error("无登录信息"));
            }

            (var list, var count) = await _iWorkFlow_InstanceStepBll.Query(page, limit, reviewStatus,userInfo.Id);

            return Json(ApiResulthelp.Success(list, count));
        }


        public async Task<IActionResult> Review(string Id, string reviewReason, int reviewStatus)
        {
            if(reviewStatus == 2 || reviewStatus == 3)
            {
                bool ReviewIsSuccess = await _iWorkFlow_InstanceStepBll.Review(Id, reviewReason, reviewStatus);
                if (ReviewIsSuccess)
                {
                    return Json(ApiResulthelp.Success(reviewStatus));
                }
                else
                {
                    return Json(ApiResulthelp.Error("失败"));
                }
            }
            else
            {
                return Json(ApiResulthelp.Error("参数有误"));
            }
        }

        public IActionResult QueryStatusSelectOption()
        {
            var statusList = new List<Object>();
            statusList.Add(new
            {
                Key = "审批中",
                Value = 1
            });
            statusList.Add(new
            {
                Key = "通过",
                Value = 2
            });
            statusList.Add(new
            {
                Key = "驳回",
                Value = 3
            });
            statusList.Add(new
            {
                Key = "作废",
                Value = 4
            });

            return Json(ApiResulthelp.Success(statusList));
        }

    }
}
