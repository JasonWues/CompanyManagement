using Entity;
using ICompanyDal;

namespace CompanyDal
{
    public class WorkFlow_ModelDal : BaseDal<WorkFlow_Model>, IWorkFlow_ModelDal
    {
        readonly CompanyContext _companyContext;
        public WorkFlow_ModelDal(CompanyContext companyContext) : base(companyContext)
        {
            _companyContext = companyContext;
        }
    }
}
