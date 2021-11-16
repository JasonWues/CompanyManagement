using Entity;
using Entity.DTO;
using ICompanyBll;
using ICompanyDal;
using Microsoft.EntityFrameworkCore;
using Utility.ExtendMethod;

namespace CompanyBll
{
    public class RRoleInfoMenuInfoBll : BaseBll<R_RoleInfo_MenuInfo>,IRRoleInfoMenuInfoBll
    {
        public RRoleInfoMenuInfoBll(IRRoleInfoMenuInfoDal iRRoleInfoMenuInfoDal)
        {
            base._iBaseDal = iRRoleInfoMenuInfoDal;
        }
    }
}
