using Entity;
using ICompanyBll;
using ICompanyDal;

namespace CompanyBll
{
    public class R_UserInfo_RoleInfoBll : BaseBll<R_UserInfo_RoleInfo>,IR_UserInfo_RoleInfoBll
    {
        public R_UserInfo_RoleInfoBll(IRUserInfo_RoleInfoDal iRuserInfo_RoleInfoDal)
        {
            base._iBaseDal = iRuserInfo_RoleInfoDal;
        }

        public async Task<bool> Update(string Id, string userid, string roleid)
        {
            R_UserInfo_RoleInfo RuserInfoRoleInfo = await _iBaseDal.Find(Id);
            RuserInfoRoleInfo.UserId = userid;
            RuserInfoRoleInfo.RoleId = roleid;
            return await _iBaseDal.Update(RuserInfoRoleInfo);
        }
    }
}
