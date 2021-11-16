using Entity;
using ICompanyDal;

namespace CompanyDal
{
    public class RuserInfo_RoleInfoDal : BaseDal<R_UserInfo_RoleInfo>, IRUserInfo_RoleInfoDal
    {
        readonly CompanyContext _companyContext;
        public RuserInfo_RoleInfoDal(CompanyContext companyContext) : base(companyContext)
        {
            _companyContext = companyContext;
        }
    }
}
