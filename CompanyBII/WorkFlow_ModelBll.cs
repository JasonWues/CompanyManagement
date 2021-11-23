using Entity;
using ICompanyBll;
using ICompanyDal;

namespace CompanyBll
{
    public class WorkFlow_ModelBll : BaseBll<WorkFlow_Model>, IWorkFlow_ModelBll
    {
        public WorkFlow_ModelBll(IWorkFlow_ModelDal iWorkFlow_ModelDal)
        {
            _iBaseDal = iWorkFlow_ModelDal;
        }
    }
}
