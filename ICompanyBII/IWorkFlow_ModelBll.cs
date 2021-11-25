using Entity;

namespace ICompanyBll
{
    public interface IWorkFlow_ModelBll : IBaseBll<WorkFlow_Model>
    {
        public Task<bool> Update(string Id, string title, string description);
    }
}
