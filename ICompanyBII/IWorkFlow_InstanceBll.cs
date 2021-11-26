using Entity;
using Entity.DTO;

namespace ICompanyBll
{
    public interface IWorkFlow_InstanceBll : IBaseBll<WorkFlow_Instance>
    {
        public Task<(List<UserInfo_ConsumableInfo_WorkFlowModel_WorkFlowInstanc> list, int count)> Query(int page, int limit, int status);
        public Task<bool> Create(WorkFlow_Instance entity, string departmentId);
    }
}
