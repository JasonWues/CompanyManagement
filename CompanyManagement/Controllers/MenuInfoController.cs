using Entity;
using Entity.DTO;
using ICompanyBll;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Utility.ApiResult;

namespace CompanyManagement.Controllers
{
    public class MenuInfoController : Controller
    {
        readonly IMenuInfoBll _iMenuInfoBll;
        public MenuInfoController(IMenuInfoBll iMenuInfoBll)
        {
            _iMenuInfoBll = iMenuInfoBll;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetMenuInfoJson()
        {
            string userInfoJson = HttpContext.Session.GetString("UserInfo");
            UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(userInfoJson);
            return Json(new
            {
                homeInfo = new
                {
                    title = "首页",
                    href = ""
                },
                logoInfo = new
                {
                    title = "Metatron",
                    image = "../../layuimini/layuimini-2.0.6.1-iframe/images/logo.png",
                    href = ""
                },
                menuInfo = new List<ParentMenuInfo>()
                {
                    new ParentMenuInfo()
                    {
                        Title = "常规管理",
                        Icon = "fa fa-address-book",
                        Href = "",
                        Target = "_self",
                        child = await _iMenuInfoBll.GetMenuInfoJson(userInfo)
                    }
                }
            });
        }

        public async Task<IActionResult> Create(string title, string description, int level, int sort, string href, string parentId, string icon, string target)
        {
            MenuInfo menuInfo = new MenuInfo();
            menuInfo.Id = Guid.NewGuid().ToString();
            menuInfo.Title = title;
            menuInfo.Description = description;
            menuInfo.Level = level;
            menuInfo.Sort = sort;
            menuInfo.Href = href;
            menuInfo.ParentId = parentId;
            menuInfo.Icon = icon;
            menuInfo.Target = target;
            menuInfo.CreateTime = DateTime.Now;
            var b = await _iMenuInfoBll.Create(menuInfo);
            if (b)
            {
                return Json(ApiResulthelp.Success("添加成功"));
            }
            else
            {
                return Json(ApiResulthelp.Error("失败"));
            }
        }

        public IActionResult CreateView()
        {
            return View();
        }

        public async Task<IActionResult> Query(string title, int page, int limit)
        {
            (List<MenuInfo_MenuInfo> list, int count) = await _iMenuInfoBll.Query(title, page, limit);
            return Json(ApiResulthelp.Success(list, count));
        }

        public IActionResult QueryMenuInfoTitle(string Id)
        {
            var currentMenuInfoTitle = _iMenuInfoBll.QueryDb().Where(x => x.Id == Id).ToList();

            var menInfo = _iMenuInfoBll.QueryDb().Where(x => x.IsDelete == false && x.Id != Id).Select(x => new
            {
                x.Id,
                x.Title
            });

            return Json(new
            {
                currentMenuInfoTitle,
                menInfo
            });
        }

        public IActionResult UpdateView()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, string title, string description, int level, int sort, string href, string parentId, string icon, string target)
        {
            var b = await _iMenuInfoBll.Update(id, title, description, level, sort, href, parentId, icon, target);
            if (b) return Json(ApiResulthelp.Success(b));
            return Json(ApiResulthelp.Error("错误"));
        }

        public async Task<IActionResult> Delete(List<string> Id)
        {
            string jsonUserInfo = HttpContext.Session.GetString("UserInfo");
            UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(jsonUserInfo);

            if (userInfo == null)
            {
                return Json(ApiResulthelp.Error("无登录信息"));
            }
            else
            {
                if (userInfo.isAdmin == 0)
                {
                    return Json(ApiResulthelp.Error("当前账号不是管理员"));
                }
                else
                {
                    int b = await _iMenuInfoBll.FakeDelete(x => Id.Contains(x.Id) && x.IsDelete == false, x => new MenuInfo()
                    {
                        IsDelete = true,
                        DeleteTime = DateTime.Now
                    });

                    if (b > 0) return Json(ApiResulthelp.Success(b));
                    return Json(ApiResulthelp.Error("删除失败"));
                }
            }

        }
    }
}
