using Entity;
using ICompanyDal;

namespace CompanyDal
{
    public class WorkFlow_InstanceStepDal : BaseDal<WorkFlow_InstanceStep>, IWorkFlow_InstanceStepDal
    {
        readonly CompanyContext _companyContext;
        public WorkFlow_InstanceStepDal(CompanyContext companyContext) : base(companyContext)
        {
            _companyContext = companyContext;
        }
    }
}
