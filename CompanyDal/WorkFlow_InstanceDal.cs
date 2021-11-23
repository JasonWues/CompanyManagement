using Entity;
using ICompanyDal;

namespace CompanyDal
{
    public class WorkFlow_InstanceDal : BaseDal<WorkFlow_Instance>, IWorkFlow_InstanceDal
    {
        readonly CompanyContext _companyContext;
        public WorkFlow_InstanceDal(CompanyContext companyContext) : base(companyContext)
        {
            _companyContext = companyContext;
        }
    }
}
