using Entity;
using Entity.DTO;
using ICompanyBll;
using ICompanyDal;

namespace CompanyBll
{
    public class WorkFlow_InstanceBll : BaseBll<WorkFlow_Instance>,IWorkFlow_InstanceBll
    {
        readonly IWorkFlow_ModelDal _iWorkFlow_ModelDal;
        readonly IConsumableInfoDal _iConsumableInfoDal;
        public WorkFlow_InstanceBll(IWorkFlow_InstanceDal iWorkFlow_InstanceDal, IWorkFlow_ModelDal iWorkFlow_ModelDall, IConsumableInfoDal iConsumableInfoDal)
        {
            _iBaseDal = iWorkFlow_InstanceDal;
            _iWorkFlow_ModelDal = iWorkFlow_ModelDall;
            _iConsumableInfoDal = iConsumableInfoDal;
        }

        public Task<(List<UserInfo_ConsumableInfo_WorkFlowModel_WorkFlowInstanc> list, int count)> Query(string consumableName, int page, int limit)
        {

            var list = from wi in _iBaseDal.QueryDb().AsQueryable()
                       join wm in _iWorkFlow_ModelDal.QueryDb().Where(x => x.isDelete == false)
                       on wi.ModelId equals wm.Id into grouping
                       from wiwm in grouping.DefaultIfEmpty()
                       join x in _iConsumableInfoDal.QueryDb().Where(x => x.IsDelete == false)
                       on wi.OutGoodsId equals x.Id into d
                       from g in d.DefaultIfEmpty()
                       select new UserInfo_ConsumableInfo_WorkFlowModel_WorkFlowInstanc
                       {
                           Id = wi.Id,
                           ModelTitle = wiwm.Title,
                           ConsumableName = g.Name,
                           OutNum = wi.OutNum,
                           CreateTime = DateTime.Now.ToString("g")
                       };
                       

        }
    }
}
