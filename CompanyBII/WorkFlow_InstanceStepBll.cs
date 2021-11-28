using Entity;
using Entity.DTO;
using ICompanyBll;
using ICompanyDal;
using Microsoft.EntityFrameworkCore;
using Utility.ExtendMethod;

namespace CompanyBll
{
    public class WorkFlow_InstanceStepBll : BaseBll<WorkFlow_InstanceStep>,IWorkFlow_InstanceStepBll
    {
        readonly IWorkFlow_InstanceDal _iWorkFlow_InstanceDal;
        readonly IWorkFlow_ModelDal _iWorkFlow_ModelDal;
        readonly IUserInfoDal _iUserInfoDal;
        readonly IRoleInfoDal _iRoleInfoDal;
        readonly IRUserInfo_RoleInfoDal _iRUserInfo_RoleInfoDal;
        readonly CompanyContext _companyContext;

        public WorkFlow_InstanceStepBll(IWorkFlow_InstanceStepDal iWorkFlow_InstanceStepDal, IWorkFlow_InstanceDal iWorkFlow_InstanceDal, IWorkFlow_ModelDal iWorkFlow_ModelDal, IUserInfoDal iUserInfoDal, IRoleInfoDal iRoleInfoDal, IRUserInfo_RoleInfoDal iRUserInfo_RoleInfoDal, CompanyContext companyContext)
        {
            _iBaseDal = iWorkFlow_InstanceStepDal;
            _iWorkFlow_InstanceDal = iWorkFlow_InstanceDal;
            _iWorkFlow_ModelDal = iWorkFlow_ModelDal;
            _iUserInfoDal = iUserInfoDal;
            _iRUserInfo_RoleInfoDal = iRUserInfo_RoleInfoDal;
            _iRoleInfoDal = iRoleInfoDal;
            _companyContext = companyContext;

        }

        public async Task<(List<WorkFlowInstanceSteps_WorkFlowModel_UserInfo> list,int count)> Query(int page, int limit, int reviewStatus, string userInfoId)
        {

            var workFlowInstancesSteps = _iBaseDal.QueryDb().Where(x => x.ReviewerId == userInfoId);
            int count = 0;

            if(reviewStatus != 0)
            {
                workFlowInstancesSteps = workFlowInstancesSteps.Where(x => x.ReviewStatus == reviewStatus);
            }

            count = workFlowInstancesSteps.Count();

            var list = await (from wis in workFlowInstancesSteps
                       join wi in _iWorkFlow_InstanceDal.QueryDb()
                       on wis.InstanceId equals wi.Id into grouping
                       from wiswi in grouping.DefaultIfEmpty()

                       join wm in _iWorkFlow_ModelDal.QueryDb().Where(x => x.isDelete == false)
                       on wiswi.ModelId equals wm.Id

                       join u in _iUserInfoDal.QueryDb().Where(x => x.IsDelete == false)
                       on wis.ReviewerId equals u.Id into grouping2
                       from uw in grouping2.DefaultIfEmpty()

                       select new WorkFlowInstanceSteps_WorkFlowModel_UserInfo()
                       {
                           Id = wis.Id,
                           ReviewStatus = wis.ReviewStatus.ConvertStatus(),
                           Title = wm.Title,
                           UserName = uw.UserName,
                           ReviewReason = wis.ReviewReason,
                           CreateTime = wis.CreateTime.ToString("g"),
                           ReviewTime = wis.ReviewTime.ToString("g") == null ? "" : wis.ReviewTime.ToString("g")
                       }).OrderBy(x => x.ReviewStatus).Skip((page-1)* limit).Take(limit).ToListAsync();

            count = list.Count;

            return (list, count);

        }

        public async Task<bool> Review(string stepId, string reviewReason, int reviewStatus)
        {
            var workFlowInstanceStep = await _iBaseDal.Find(stepId);
            List<WorkFlow_InstanceStep> workFlow_InstanceSteps = new List<WorkFlow_InstanceStep>();
            if(workFlowInstanceStep != null)
            {
                if(workFlowInstanceStep.ReviewStatus == 2 || workFlowInstanceStep.ReviewStatus == 3)
                {
                    return false;
                }

                workFlowInstanceStep.ReviewReason = reviewReason;
                workFlowInstanceStep.ReviewStatus = reviewStatus;
                workFlowInstanceStep.ReviewTime = DateTime.Now;
            }

            if(reviewStatus == 2)
            {
                using (var transaction = await _companyContext.Database.BeginTransactionAsync())
                {
                    bool UpdateIsSuccess = await _iBaseDal.Update(workFlowInstanceStep);

                    var userInfoIds = await (from ur in _iRUserInfo_RoleInfoDal.QueryDb()
                                       join r in _iRoleInfoDal.QueryDb().Where(x => x.IsDelete == false && x.RoleName == "仓库管理员")
                                       on ur.RoleId equals r.Id
                                       select ur.UserId).ToListAsync();

                    if(userInfoIds.Count == 0)
                    {
                        return false;
                    }

                    foreach (var item in userInfoIds)
                    {
                        workFlow_InstanceSteps.Add(new WorkFlow_InstanceStep()
                        {
                            Id = Guid.NewGuid().ToString(),
                            BeforeStepId = workFlowInstanceStep.Id,
                            CreateTime = DateTime.Now,
                            InstanceId = workFlowInstanceStep.InstanceId,
                            ReviewerId = item,
                            ReviewStatus = 1
                        });
                    }



                    if(UpdateIsSuccess && workFlow_InstanceSteps.Count > 0)
                    {
                        await _iBaseDal.QueryDb().AddRangeAsync(workFlow_InstanceSteps);
                        await transaction.CommitAsync();
                        return true;
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                }
            }
            else if(reviewStatus == 3)
            {
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
