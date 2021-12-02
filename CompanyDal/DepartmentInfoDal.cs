using Entity;
using ICompanyDal;

namespace CompanyDal
{
    public class DepartmentInfoDal : BaseDal<DepartmentInfo>, IDepartmentInfoDal
    {
        readonly CompanyContext _companyContext;
        public DepartmentInfoDal(CompanyContext companyContext) : base(companyContext)
        {
            _companyContext = companyContext;
        }
    }
}
