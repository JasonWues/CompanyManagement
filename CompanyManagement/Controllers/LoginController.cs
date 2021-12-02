using Entity;
using ICompanyBll;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Utility;
using Utility.ApiResult;

namespace CompanyManagement.Controllers;

public class LoginController : Controller
{
    private IUserInfoBll _iuserInfoBll;

    public LoginController(IUserInfoBll iuserInfoBll)
    {
        _iuserInfoBll = iuserInfoBll;
    }
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult LoginVali(string account, string password)
    {
        if (string.IsNullOrEmpty(account))
        {
            return Json(ApiResulthelp.Error("账号为空"));
        }
        if (string.IsNullOrEmpty(password))
        {
            return Json(ApiResulthelp.Error("密码为空"));
        }

        var userinfo = _iuserInfoBll.QueryDb().Where(x => x.Account == account && x.PassWord == MD5.MD5Encrypt16(password) && x.IsDelete == false).FirstOrDefault();
        if (userinfo != null)
        {
            string jsonUserInfo = JsonConvert.SerializeObject(userinfo);
            HttpContext.Session.SetString("UserInfo", jsonUserInfo);
            return Json(ApiResulthelp.Success(userinfo.UserName));
        }
        else
        {
            return Json(ApiResulthelp.Error("没有这个用户"));
        }
    }

    public IActionResult IsAdmin()
    {
        string jsonUserInfo = HttpContext.Session.GetString("UserInfo");
        UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(jsonUserInfo);

        if (userInfo != null)
        {
            if (userInfo.IsAdmin == 1)
            {
                return Json(ApiResulthelp.Success("管理员"));
            }
            else
            {
                return Json(ApiResulthelp.Error("不是管理员"));
            }
        }
        else
        {
            return Json(ApiResulthelp.Error("无数据"));
        }

    }

    public void Logout()
    {
        HttpContext.Session.Clear();
    }
}