using Entity;

namespace ICompanyBll
{
    public interface IR_UserInfo_RoleInfoBll : IBaseBll<R_UserInfo_RoleInfo>
    {
        public Task<bool> Update(string Id, string userid, string roleid);

    }
}
