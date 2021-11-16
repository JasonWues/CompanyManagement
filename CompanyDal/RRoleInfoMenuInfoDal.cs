using Entity;
using ICompanyDal;

namespace CompanyDal
{
    public class RRoleInfoMenuInfoDal : BaseDal<R_RoleInfo_MenuInfo>, IRRoleInfoMenuInfoDal
    {
        readonly CompanyContext _companyContext;
        public RRoleInfoMenuInfoDal(CompanyContext companyContext) : base(companyContext)
        {
            _companyContext = companyContext;
        }
    }
}
