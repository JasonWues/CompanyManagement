using EFCore.BulkExtensions;
using Entity;
using Entity.DTO;
using ICompanyBll;
using ICompanyDal;
using Microsoft.EntityFrameworkCore;
using Utility.ExtendMethod;

namespace CompanyBll
{
    public class WorkFlow_InstanceBll : BaseBll<WorkFlow_Instance>, IWorkFlow_InstanceBll
    {
        readonly IWorkFlow_ModelDal _iWorkFlow_ModelDal;
        readonly IConsumableInfoDal _iConsumableInfoDal;
        readonly IUserInfoDal _iUserInfoDal;
        readonly IDepartmentInfoDal _iDepartmentInfoDal;
        readonly IWorkFlow_InstanceStepDal _iWorkFlow_InstanceStepDal;
        readonly CompanyContext _companyContext;
        public WorkFlow_InstanceBll(IWorkFlow_InstanceDal iWorkFlow_InstanceDal, IWorkFlow_ModelDal iWorkFlow_ModelDall, IConsumableInfoDal iConsumableInfoDal, IUserInfoDal iUserInfoDal, IDepartmentInfoDal iDepartmentInfoDal, IWorkFlow_InstanceStepDal iWorkFlow_InstanceStepDal, CompanyContext companyContext)
        {
            _iBaseDal = iWorkFlow_InstanceDal;
            _iWorkFlow_ModelDal = iWorkFlow_ModelDall;
            _iConsumableInfoDal = iConsumableInfoDal;
            _iUserInfoDal = iUserInfoDal;
            _iDepartmentInfoDal = iDepartmentInfoDal;
            _iWorkFlow_InstanceStepDal = iWorkFlow_InstanceStepDal;
            _companyContext = companyContext;
        }

        public async Task<(List<UserInfo_ConsumableInfo_WorkFlowModel_WorkFlowInstanc> list, int count)> Query(int page, int limit, int status, UserInfo userInfo)
        {

            var WorkFlowInstances = _iBaseDal.QueryDb().AsQueryable();

            if (userInfo.IsAdmin != 1)
            {
                WorkFlowInstances = _iBaseDal.QueryDb().AsQueryable().Where(x => x.Creator == userInfo.Id);
            }

            if (status != 0)
            {
                WorkFlowInstances = WorkFlowInstances.Where(x => x.Status == status);
            }

            int count = WorkFlowInstances.Count();


            var list = await (from wi in WorkFlowInstances
                              join wm in _iWorkFlow_ModelDal.QueryDb().Where(x => x.IsDelete == false)
                              on wi.ModelId equals wm.Id into grouping
                              from wiwm in grouping.DefaultIfEmpty()

                              join x in _iConsumableInfoDal.QueryDb().Where(x => x.IsDelete == false)
                              on wi.OutGoodsId equals x.Id into d
                              from g in d.DefaultIfEmpty()

                              join u in _iUserInfoDal.QueryDb().Where(x => x.IsDelete == false)
                              on wi.Creator equals u.Id into v
                              from h in v.DefaultIfEmpty()
                              select new UserInfo_ConsumableInfo_WorkFlowModel_WorkFlowInstanc
                              {
                                  Id = wi.Id,
                                  ModelTitle = wiwm.Title,
                                  ConsumableName = g.Name,
                                  Creator = wi.Creator,
                                  Description = wi.Description,
                                  Reason = wi.Reason,
                                  OutNum = wi.OutNum,
                                  UserName = h.UserName,
                                  Status = wi.Status.ConvertStatus(),
                                  CreateTime = DateTime.Now.ToString("g")
                              }).OrderBy(x => x.OutNum).Skip((page - 1) * limit).Take(limit).ToListAsync();

            count = list.Count;

            return (list, count);
        }

        public async Task<(bool isSuccess, string msg)> Cancel(string Id, UserInfo userInfo)
        {
            WorkFlow_Instance workFlow_Instance = await _iBaseDal.Find(Id);

            if (userInfo.IsAdmin != 1)
            {
                if (workFlow_Instance.Creator == userInfo.Id)
                {
                    return (false, "不是本人流程");
                }
            }

            if (workFlow_Instance == null)
            {
                return (false, "无实例信息");
            }


            workFlow_Instance.Status = 4;

            using (var transaction = await _companyContext.Database.BeginTransactionAsync())
            {
                bool workFlowInstanceUpdateIsSuccess = await _iBaseDal.Update(workFlow_Instance);

                var step = await _iWorkFlow_InstanceStepDal.Query(x => x.InstanceId == Id);

                foreach (var item in step)
                {
                    if (item.ReviewStatus == 1)
                    {
                        item.ReviewStatus = 4;
                    }
                }

                bool workFlowInstanceStepUpdateIsSuccess = await _iWorkFlow_InstanceStepDal.QueryDb().BatchUpdateAsync(workFlow_Instance) > 0 ? true : false;

                if (workFlowInstanceUpdateIsSuccess && workFlowInstanceStepUpdateIsSuccess)
                {
                    await transaction.CommitAsync();
                    return (true, "提交成功");
                }
                else
                {
                    await transaction.RollbackAsync();
                    return (false, "提交失败");
                }
            }
        }

        public async Task<bool> Create(WorkFlow_Instance entity, string departmentId)
        {
            var departmentInfo = await _iDepartmentInfoDal.QueryDb().Where(x => x.IsDelete == false).FirstOrDefaultAsync(x => x.Id == departmentId);

            if (departmentInfo == null)
            {
                return false;
            }

            WorkFlow_InstanceStep workFlow_InstanceStep = new WorkFlow_InstanceStep()
            {
                Id = Guid.NewGuid().ToString(),
                CreateTime = DateTime.Now,
                InstanceId = entity.Id,
                ReviewerId = departmentInfo.LeaderId,
                ReviewStatus = 1
            };

            using (var transaction = await _companyContext.Database.BeginTransactionAsync())
            {
                bool WorkFlow_InstanceIsSuccess = await _iBaseDal.Create(entity);
                bool WorkFlow_InstanceStepIsSuccess = await _iWorkFlow_InstanceStepDal.Create(workFlow_InstanceStep);
                if (WorkFlow_InstanceIsSuccess && WorkFlow_InstanceStepIsSuccess)
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

        }
    }
}
