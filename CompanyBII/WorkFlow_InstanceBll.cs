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
        public WorkFlow_InstanceBll(IWorkFlow_InstanceDal iWorkFlow_InstanceDal, IWorkFlow_ModelDal iWorkFlow_ModelDall, IConsumableInfoDal iConsumableInfoDal, IUserInfoDal iUserInfoDal, IDepartmentInfoDal iDepartmentInfoDal, IWorkFlow_InstanceStepDal iWorkFlow_InstanceStepDal)
        {
            _iBaseDal = iWorkFlow_InstanceDal;
            _iWorkFlow_ModelDal = iWorkFlow_ModelDall;
            _iConsumableInfoDal = iConsumableInfoDal;
            _iUserInfoDal = iUserInfoDal;
            _iDepartmentInfoDal = iDepartmentInfoDal;
            _iWorkFlow_InstanceStepDal = iWorkFlow_InstanceStepDal;
        }

        public async Task<(List<UserInfo_ConsumableInfo_WorkFlowModel_WorkFlowInstanc> list, int count)> Query(int page, int limit, int status)
        {
            var WorkFlowInstances = _iBaseDal.QueryDb().AsQueryable();

            if (status != 0)
            {
                WorkFlowInstances = WorkFlowInstances.Where(x => x.Status == status);
            }

            int count = WorkFlowInstances.Count();


            var list = await (from wi in WorkFlowInstances
                              join wm in _iWorkFlow_ModelDal.QueryDb().Where(x => x.isDelete == false)
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

            count = list.Count();

            return (list, count);
        }

        public async Task<bool> Create(WorkFlow_Instance entity, string departmentId)
        {
            var leaderIds = await _iDepartmentInfoDal.QueryDb().Where(x => x.IsDelete == false && x.Id == departmentId).Select(x => x.LeaderId).ToListAsync();

            if (leaderIds.Count() == 0)
            {
                return false;
            }

            List<WorkFlow_InstanceStep> workFlow_InstanceSteps = new List<WorkFlow_InstanceStep>();

            foreach (var item in leaderIds)
            {
                workFlow_InstanceSteps.Add(new WorkFlow_InstanceStep()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreateTime = DateTime.Now,
                    InstanceId = entity.Id,
                    ReviewerId = item,
                    ReviewStatus = 1
                });
            }

            if (workFlow_InstanceSteps.Count() > 0 && entity != null)
            {
                await _iWorkFlow_InstanceStepDal.BatchInsert(workFlow_InstanceSteps);
                bool workFlow_InstanceisSuccess = await _iBaseDal.Create(entity);
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
