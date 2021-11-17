using Entity;
using Entity.DTO;

namespace ICompanyBll;

public interface IMenuInfoBll : IBaseBll<MenuInfo>
{

    public Task<List<ParentMenuInfo>> GetMenuInfoJson(UserInfo Userinfo);

    public Task<(List<MenuInfo_MenuInfo> list, int count)> Query(string title, int page, int limit);

    public Task<bool> Update(string id, string title, string description, int level, int sort, string href, string parentId, string icon, string target)
}