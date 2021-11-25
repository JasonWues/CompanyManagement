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

        public async Task<bool> Update(string Id, string title, string description)
        {
            WorkFlow_Model workFlow_Model = await _iBaseDal.Find(Id);
            workFlow_Model.Title = title;
            workFlow_Model.Description = description;
            var b = await _iBaseDal.Update(workFlow_Model);
            return b;
        }
    }
}
