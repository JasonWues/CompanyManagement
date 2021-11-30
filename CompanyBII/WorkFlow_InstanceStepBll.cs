using Entity;
using Entity.DTO;
using ICompanyBll;
using ICompanyDal;
using Microsoft.EntityFrameworkCore;
using Utility.ExtendMethod;

namespace CompanyBll
{
    public class WorkFlow_InstanceStepBll : BaseBll<WorkFlow_InstanceStep>, IWorkFlow_InstanceStepBll
    {
        readonly IWorkFlow_InstanceDal _iWorkFlow_InstanceDal;
        readonly IWorkFlow_ModelDal _iWorkFlow_ModelDal;
        readonly IUserInfoDal _iUserInfoDal;
        readonly IRoleInfoDal _iRoleInfoDal;
        readonly IRUserInfo_RoleInfoDal _iRUserInfo_RoleInfoDal;
        readonly IConsumableInfoDal _iConsumableInfoDal;
        readonly IDepartmentInfoDal _iDepartmentInfoDal;
        readonly CompanyContext _companyContext;

        public WorkFlow_InstanceStepBll(IWorkFlow_InstanceStepDal iWorkFlow_InstanceStepDal, IWorkFlow_InstanceDal iWorkFlow_InstanceDal, IWorkFlow_ModelDal iWorkFlow_ModelDal, IUserInfoDal iUserInfoDal, IRoleInfoDal iRoleInfoDal, IRUserInfo_RoleInfoDal iRUserInfo_RoleInfoDal, IConsumableInfoDal iConsumableInfoDal, IDepartmentInfoDal iDepartmentInfoDal, CompanyContext companyContext)
        {
            _iBaseDal = iWorkFlow_InstanceStepDal;
            _iWorkFlow_InstanceDal = iWorkFlow_InstanceDal;
            _iWorkFlow_ModelDal = iWorkFlow_ModelDal;
            _iUserInfoDal = iUserInfoDal;
            _iRUserInfo_RoleInfoDal = iRUserInfo_RoleInfoDal;
            _iRoleInfoDal = iRoleInfoDal;
            _iConsumableInfoDal = iConsumableInfoDal;
            _iDepartmentInfoDal = iDepartmentInfoDal;
            _companyContext = companyContext;

        }

        public async Task<(List<WorkFlowInstanceSteps_WorkFlowModel_UserInfo> list, int count)> Query(int page, int limit, int reviewStatus, string userInfoId)
        {

            var workFlowInstancesSteps = _iBaseDal.QueryDb().Where(x => x.ReviewerId == userInfoId);
            int count = 0;

            if (reviewStatus != 0)
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

                              join u2 in _iUserInfoDal.QueryDb().Where(x => x.IsDelete == false)
                              on wiswi.Creator equals u2.Id into grouping3
                              from uw2 in grouping3.DefaultIfEmpty()

                              join c in _iConsumableInfoDal.QueryDb().Where(x => x.IsDelete == false)
                              on wiswi.OutGoodsId equals c.Id into grouping4
                              from cws in grouping4.DefaultIfEmpty()

                              select new WorkFlowInstanceSteps_WorkFlowModel_UserInfo()
                              {
                                  Id = wis.Id,
                                  ReviewStatus = wis.ReviewStatus.ConvertStatus(),
                                  Title = wm.Title,
                                  UserName = uw.UserName,
                                  ReviewReason = wis.ReviewReason,
                                  CreatorName = uw2.UserName,
                                  //OutInt = wiswi.OutNum,
                                  CreateTime = wis.CreateTime.ToString("g"),
                                  ReviewTime = wis.ReviewTime.ToString("g") == null ? "" : wis.ReviewTime.ToString("g")
                              }).OrderBy(x => x.ReviewStatus).Skip((page - 1) * limit).Take(limit).ToListAsync();

            count = list.Count;

            return (list, count);

        }

        public async Task<bool> Review(string stepId, string reviewReason, int reviewStatus)
        {
            var workFlowInstanceStep = await _iBaseDal.Find(stepId);
            List<WorkFlow_InstanceStep> workFlow_InstanceSteps = new List<WorkFlow_InstanceStep>();
            if (workFlowInstanceStep != null)
            {
                if (workFlowInstanceStep.ReviewStatus == 2 || workFlowInstanceStep.ReviewStatus == 3)
                {
                    return false;
                }

                workFlowInstanceStep.ReviewReason = reviewReason;
                workFlowInstanceStep.ReviewStatus = reviewStatus;
                workFlowInstanceStep.ReviewTime = DateTime.Now;
            }

            if (reviewStatus == 2)
            {
                using (var transaction = await _companyContext.Database.BeginTransactionAsync())
                {
                    bool workFlowInstanceStepUpdateIsSuccess = await _iBaseDal.Update(workFlowInstanceStep);

                    var userInfoIds = await (from ur in _iRUserInfo_RoleInfoDal.QueryDb()
                                             join r in _iRoleInfoDal.QueryDb().Where(x => x.IsDelete == false && x.RoleName == "仓库管理员")
                                             on ur.RoleId equals r.Id
                                             select ur.UserId).ToListAsync();

                    bool isStorehouseAdmin = userInfoIds.Any(x => x == workFlowInstanceStep.ReviewerId);


                    if (isStorehouseAdmin)
                    {
                        var workFlowInstance = await _iWorkFlow_InstanceDal.Find(workFlowInstanceStep.InstanceId);

                        if (workFlowInstance != null)
                        {
                            return false;
                        }

                        workFlowInstance.Status = 2;

                        bool workFlowInstanceUpdateIsSuccess = await _iWorkFlow_InstanceDal.Update(workFlowInstance);

                        var consumableInfo = await _iConsumableInfoDal.Find(workFlowInstance.OutGoodsId);

                        if (consumableInfo.Num < workFlowInstance.OutNum)
                        {
                            await transaction.RollbackAsync();
                            return false;
                        }

                        consumableInfo.Num -= workFlowInstance.OutNum;

                        bool consumableInfoUpdateIsSuccess = await _iConsumableInfoDal.Update(consumableInfo);

                        if (consumableInfoUpdateIsSuccess && workFlowInstanceUpdateIsSuccess && workFlowInstanceStepUpdateIsSuccess)
                        {
                            await transaction.CommitAsync();
                            return true;
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                            return false;
                        }

                    }
                    else
                    {

                        var leaderId = await (from wi in _iWorkFlow_InstanceDal.QueryDb().Where(x => x.Id == workFlowInstanceStep.InstanceId)
                                              join u in _iUserInfoDal.QueryDb().Where(x => x.IsDelete == false)
                                              on wi.Creator equals u.Id

                                              join d in _iDepartmentInfoDal.QueryDb().Where(x => x.IsDelete == false)
                                              on u.DepartmentId equals d.Id
                                              select d.LeaderId).FirstOrDefaultAsync();

                        if(string.IsNullOrEmpty(leaderId) || workFlowInstanceStep.ReviewerId != leaderId)
                        {
                            await transaction.RollbackAsync();
                            return false;
                        }



                        if (userInfoIds.Count == 0)
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


                        if (workFlowInstanceStepUpdateIsSuccess && workFlow_InstanceSteps.Count > 0)
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
            }
            else if (reviewStatus == 3)
            {
                using (var transaction = await _companyContext.Database.BeginTransactionAsync())
                {
                    bool workFlowInstanceStepUpdateIsSuccess = await _iBaseDal.Update(workFlowInstanceStep);

                    WorkFlow_Instance workFlow_Instance = await _iWorkFlow_InstanceDal.Find(workFlowInstanceStep.InstanceId);

                    if (workFlow_Instance.Status == 2)
                    {
                        workFlow_Instance.Status = 3;
                    }
                    bool workFlowInstanceUpdateIsSuccess = await _iWorkFlow_InstanceDal.Update(workFlow_Instance);

                    if(workFlowInstanceStepUpdateIsSuccess && workFlowInstanceUpdateIsSuccess)
                    {
                        return true;
                        await transaction.CommitAsync();
                    }
                    else
                    {
                        return false;
                        await transaction.RollbackAsync();
                    }
                }       
            }
            else
            {
                return false;
            }
        }
    }
}
