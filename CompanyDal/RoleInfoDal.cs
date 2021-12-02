using Entity;
using ICompanyDal;

namespace CompanyDal
{
    public class RoleInfoDal : BaseDal<RoleInfo>, IRoleInfoDal
    {
        readonly CompanyContext _companyContext;
        public RoleInfoDal(CompanyContext companyContext) : base(companyContext)
        {
            _companyContext = companyContext;
        }
    }
}
