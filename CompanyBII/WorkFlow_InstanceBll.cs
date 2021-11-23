using Entity;
using ICompanyBll;
using ICompanyDal;

namespace CompanyBll
{
    public class WorkFlow_InstanceBll : BaseBll<WorkFlow_Instance>,IWorkFlow_InstanceBll
    {
        public WorkFlow_InstanceBll(IWorkFlow_InstanceDal iWorkFlow_InstanceDal)
        {
            _iBaseDal = iWorkFlow_InstanceDal;
        }
    }
}
