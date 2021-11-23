using Entity;
using ICompanyBll;
using ICompanyDal;

namespace CompanyBll
{
    public class WorkFlow_InstanceStepBll : BaseBll<WorkFlow_InstanceStep>,IWorkFlow_InstanceStepBll
    {
        public WorkFlow_InstanceStepBll(IWorkFlow_InstanceStepDal iWorkFlow_InstanceStepDal)
        {
            _iBaseDal = iWorkFlow_InstanceStepDal;
        }
    }
}
