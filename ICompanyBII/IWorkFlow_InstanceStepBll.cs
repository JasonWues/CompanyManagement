using Entity;
using Entity.DTO;

namespace ICompanyBll
{
    public interface IWorkFlow_InstanceStepBll : IBaseBll<WorkFlow_InstanceStep>
    {
        public Task<(List<WorkFlowInstanceSteps_WorkFlowModel_UserInfo> list, int count)> Query(int page, int limit, int reviewStatus, UserInfo userInfo);
        public Task<(bool isSuccess, string msg)> Review(string stepId, string reviewReason, int reviewStatus, UserInfo userInfo);
    }
}
