using Entity;
using Entity.DTO;
using ICompanyBll;
using ICompanyDal;
using Microsoft.EntityFrameworkCore;

namespace CompanyBll;

public class MenuInfoBll : BaseBll<MenuInfo>,IMenuInfoBll
{
    readonly IRUserInfo_RoleInfoDal _iRUserInfo_RoleInfoDal;
    readonly IRRoleInfoMenuInfoDal _iRRoleInfoMenuInfoDal;
    readonly IRoleInfoDal _iRoleInfoDal;
    public MenuInfoBll(IMenuInfoDal iMenuInfoDal, IRUserInfo_RoleInfoDal iR_UserInfo_RoleInfoDal,IRRoleInfoMenuInfoDal iRRoleInfoMenuInfoDal,IRoleInfoDal iRoleInfoDal)
    {
        _iBaseDal = iMenuInfoDal;
        _iRUserInfo_RoleInfoDal = iR_UserInfo_RoleInfoDal;
        _iRRoleInfoMenuInfoDal = iRRoleInfoMenuInfoDal;
        _iRoleInfoDal = iRoleInfoDal;
    }

    public void RecursionMenu(List<ParentMenuInfo> parentMenuInfos,List<MainMenuInfo> menus)
    {
        foreach(var item in parentMenuInfos)
        {
            var chlidMenusItem = menus.Where(x => x.ParentId == item.Id).Select(x => new ParentMenuInfo
            {
                Id = x.Id,
                Href = x.Href,
                Icon = x.Icon,
                Target = x.Target,
                Title = x.Title
            }).ToList();

            item.child = chlidMenusItem;

            RecursionMenu(chlidMenusItem, menus);
        }
    }


    public async Task<List<ParentMenuInfo>> GetMenuInfoJson(UserInfo userInfo)
    {
        
        //找出超级管理员
         var isSuperAdmin =  await (from x in _iRUserInfo_RoleInfoDal.QueryDb().Where(x => x.UserId == userInfo.Id)
         join s in _iRoleInfoDal.QueryDb().Where(x => x.RoleName == "系统管理员")
         on x.RoleId equals s.Id
         select x.Id).CountAsync() > 0 ? true : false;

        List<MainMenuInfo> menus;

        if (userInfo.isAdmin == 1 && isSuperAdmin)
        {
            menus = _iBaseDal.QueryDb().Where(x => x.IsDelete == false).OrderBy(x => x.Sort).Select(x => new MainMenuInfo
            {
                Id = x.Id,
                Title = x.Title,
                Icon = x.Icon,
                Href = x.Href,
                Target = x.Target,
                Level = x.Level,
                ParentId = x.ParentId
            }).ToList();

            
            //找出父级菜单
            var parentMenus = menus.Where(x => x.Level == 0 && x.ParentId == null).Select(x => new ParentMenuInfo
            {
                Id = x.Id,
                Href = x.Href,
                Icon = x.Icon,
                Target = x.Target,
                Title = x.Title
            }).ToList();

            foreach (var parentMenu in parentMenus)
            {
                var childMenus = menus.Where(m => m.ParentId == parentMenu.Id).Select(x => new ParentMenuInfo
                {
                    Id = x.Id,
                    Href = x.Href,
                    Icon = x.Icon,
                    Target = x.Target,
                    Title = x.Title
                }).ToList();

                parentMenu.child = childMenus;
                RecursionMenu(parentMenu.child, menus);
            }
            return parentMenus;
        }
        else
        {
            var roleInfoId = await _iRUserInfo_RoleInfoDal.QueryDb().Where(x => x.UserId == userInfo.Id).Select(x => x.RoleId).ToListAsync();

            var menuinfoId = await _iRRoleInfoMenuInfoDal.QueryDb().Where(x => roleInfoId.Contains(x.RoleId)).Select(x => x.MenuId).ToListAsync();

            menus = await _iBaseDal.QueryDb().Where(x => menuinfoId.Contains(x.Id)).Select(x => new MainMenuInfo
            {
                Id = x.Id,
                Title = x.Title,
                Icon = x.Icon,
                Href = x.Href,
                Target = x.Target,
                Level = x.Level,
                ParentId = x.ParentId
            }).ToListAsync();

            var parentMenus = menus.Where(x => x.Level == 0 && x.ParentId == null).Select(x => new ParentMenuInfo
            {
                Id = x.Id,
                Href = x.Href,
                Icon = x.Icon,
                Target = x.Target,
                Title = x.Title
            }).ToList();

            foreach (var parentMenu in parentMenus)
            {
                var childMenus = menus.Where(m => m.ParentId == parentMenu.Id).Select(x => new ParentMenuInfo
                {
                    Id = x.Id,
                    Href = x.Href,
                    Icon = x.Icon,
                    Target = x.Target,
                    Title = x.Title
                }).ToList();

                parentMenu.child = childMenus;
                RecursionMenu(parentMenu.child, menus);
            }
            return parentMenus;

        }
             
    }

  


    public async Task<(List<MenuInfo_MenuInfo> list, int count)> Query(string title, int page, int limit)
    {
        var menuInfo = _iBaseDal.QueryDb().Where(x => x.IsDelete == false);

        var count = 0;
        if (!string.IsNullOrEmpty(title))
        {
            menuInfo = _iBaseDal.QueryDb().Where(x => x.Title.Contains(title) && x.IsDelete == false);
            count = menuInfo.Count();
        }

        var query = from x in menuInfo
                    join d in _iBaseDal.QueryDb().Where(x => x.IsDelete == false)
                    on x.ParentId equals d.Id into s
                    from c in s.DefaultIfEmpty()
                    select new MenuInfo_MenuInfo
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Description = x.Description,
                        Level = x.Level,
                        Sort = x.Sort,
                        Href = x.Href,
                        Icon = x.Icon,
                        Target = x.Target,
                        CreateTime = x.CreateTime.ToString("F"),
                        ParentTitle = c.Title,
                    };

        count = query.Count();

        query = query.OrderBy(x => x.Sort).Skip((page - 1) * limit).Take(limit);
        return (await query.ToListAsync(), count);
    }
}