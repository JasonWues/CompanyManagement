using Entity;
using Entity.DTO;
using ICompanyBll;
using ICompanyDal;
using Microsoft.EntityFrameworkCore;

namespace CompanyBll;

public class MenuInfoBll : BaseBll<MenuInfo>, IMenuInfoBll
{
    readonly IRUserInfo_RoleInfoDal _iRUserInfo_RoleInfoDal;
    readonly IRRoleInfoMenuInfoDal _iRRoleInfoMenuInfoDal;
    readonly IRoleInfoDal _iRoleInfoDal;
    public MenuInfoBll(IMenuInfoDal iMenuInfoDal, IRUserInfo_RoleInfoDal iR_UserInfo_RoleInfoDal, IRRoleInfoMenuInfoDal iRRoleInfoMenuInfoDal, IRoleInfoDal iRoleInfoDal)
    {
        _iBaseDal = iMenuInfoDal;
        _iRUserInfo_RoleInfoDal = iR_UserInfo_RoleInfoDal;
        _iRRoleInfoMenuInfoDal = iRRoleInfoMenuInfoDal;
        _iRoleInfoDal = iRoleInfoDal;
    }

    public void RecursionMenu(List<ParentMenuInfo> parentMenuInfos, List<MainMenuInfo> menus)
    {
        foreach (var item in parentMenuInfos)
        {
            var chlidMenusItem = menus.Where(x => x.ParentId == item.Id).Select(x => new ParentMenuInfo
            {
                Id = x.Id,
                Href = x.Href,
                Icon = x.Icon,
                Target = x.Target,
                Title = x.Title
            }).ToList();

            item.Child = chlidMenusItem;

            RecursionMenu(chlidMenusItem, menus);
        }
    }


    public async Task<List<ParentMenuInfo>> GetMenuInfoJson(UserInfo userInfo)
    {

        //找出超级管理员
        var isSuperAdmin = await (from x in _iRUserInfo_RoleInfoDal.QueryDb().Where(x => x.UserId == userInfo.Id)
                                  join s in _iRoleInfoDal.QueryDb().Where(x => x.RoleName == "系统管理员" && x.IsDelete == false)
                                  on x.RoleId equals s.Id
                                  select x.Id).AnyAsync();

        List<MainMenuInfo> menus = new List<MainMenuInfo>();

        if (userInfo.IsAdmin == 1 && isSuperAdmin)
        {
            //获取所有菜单集合信息
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
        }
        else
        {
            var roleInfoId = await _iRUserInfo_RoleInfoDal.QueryDb().Where(x => x.UserId == userInfo.Id).Select(x => x.RoleId).ToListAsync();

            var menuinfoId = await _iRRoleInfoMenuInfoDal.QueryDb().Where(x => roleInfoId.Contains(x.RoleId)).Select(x => x.MenuId).ToListAsync();

            menuinfoId = menuinfoId.Distinct().ToList();

            menus = (from x in await _iBaseDal.QueryDb().Where(x => menuinfoId.Contains(x.Id)).ToListAsync()
                     join menuid in menuinfoId
                     on x.Id equals menuid
                     select new
                     {
                         Id = x.Id,
                         x.Sort,
                         Title = x.Title,
                         Icon = x.Icon,
                         Href = x.Href,
                         Target = x.Target,
                         Level = x.Level,
                         ParentId = x.ParentId
                     }).OrderBy(x => x.Sort).Select(x => new MainMenuInfo
                     {
                         Id = x.Id,
                         Title = x.Title,
                         Icon = x.Icon,
                         Href = x.Href,
                         Target = x.Target,
                         Level = x.Level,
                         ParentId = x.ParentId
                     }).ToList();
        }

        //获取父级菜单集合信息
        List<ParentMenuInfo> parentMenuInfos = menus.Where(x => x.Level == 0 && x.ParentId == null).Select(x => new ParentMenuInfo
        {
            Id = x.Id,
            Href = x.Href,
            Icon = x.Icon,
            Target = x.Target,
            Title = x.Title
        }).ToList();


        foreach (var parentMenu in parentMenuInfos)
        {
            //查询父级菜单自己的自己菜单
            var childMenus = menus.Where(m => m.ParentId == parentMenu.Id).Select(x => new ParentMenuInfo
            {
                Id = x.Id,
                Href = x.Href,
                Icon = x.Icon,
                Target = x.Target,
                Title = x.Title
            }).ToList();

            parentMenu.Child = childMenus;
            RecursionMenu(parentMenu.Child, menus);

        }

        return parentMenuInfos;
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

    public async Task<bool> Update(string id, string title, string description, int level, int sort, string href, string parentId, string icon, string target)
    {
        MenuInfo menuInfo = new MenuInfo()
        {
            Id = id,
            Title = title,
            Description = description,
            Level = level,
            Sort = sort,
            Href = href,
            ParentId = parentId,
            Icon = icon,
            Target = target
        };
        return await _iBaseDal.Update(menuInfo);
    }
}