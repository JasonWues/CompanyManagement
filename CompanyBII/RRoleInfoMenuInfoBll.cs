using Entity;
using ICompanyBll;
using ICompanyDal;

namespace CompanyBll
{
    public class RRoleInfoMenuInfoBll : BaseBll<R_RoleInfo_MenuInfo>, IRRoleInfoMenuInfoBll
    {
        public RRoleInfoMenuInfoBll(IRRoleInfoMenuInfoDal iRRoleInfoMenuInfoDal)
        {
            base._iBaseDal = iRRoleInfoMenuInfoDal;
        }
    }
}
