using Entity;
using Entity.DTO;
using ICompanyBll;
using ICompanyDal;
using Microsoft.EntityFrameworkCore;

namespace CompanyBll;

public class ConsumableRecordBll : BaseBll<ConsumableRecord>,IConsumableRecordBll
{
    readonly IUserInfoDal _iUserInfoDal;
    readonly IConsumableInfoDal _iConsumableInfoDal;
    public ConsumableRecordBll(IConsumableRecordDal iConsumableRecordDal, IUserInfoDal iUserInfoDal, IConsumableInfoDal iConsumableInfoDal)
    {
        _iBaseDal = iConsumableRecordDal;
        _iUserInfoDal = iUserInfoDal;
        _iConsumableInfoDal = iConsumableInfoDal;
    }

    public async Task<(List<Record_ConsumableInfo_UserInfo> list, int count)> Query(int page, int limit, string consumableId)
    {
        var consumableRecord = _iBaseDal.QueryDb().AsQueryable();
        var count = 0;
        if (!string.IsNullOrEmpty(consumableId))
        {
            consumableRecord = _iBaseDal.QueryDb().Where(x => x.ConsumableId == consumableId);
            count = consumableRecord.Count();
        }

        var userInfo = _iUserInfoDal.QueryDb().Where(x => x.IsDelete == false);
        var consumableInfo = _iConsumableInfoDal.QueryDb().Where(x => x.IsDelete == false);

        var query = from record in consumableRecord
                     join consumable in consumableInfo
                     on record.ConsumableId equals consumable.Id into record_consumable
                     from cc in record_consumable.DefaultIfEmpty()

                     join u in userInfo
                     on record.Creator equals u.Id into record_Userinfo
                     from ccu in record_Userinfo.DefaultIfEmpty()
                     select new Record_ConsumableInfo_UserInfo
                     {
                         Id = cc.Id,
                         CreateTime = cc.CreateTime.ToString("f"),
                         Num = cc.Num,
                         Type = record.Type == 1 ? "入库" : "出库",
                         ConsumableName = cc.Name,
                         UserName = ccu.UserName
                     };

        count = query.Count();

        query = query.OrderBy(x => x.UserName).Skip((page - 1) * limit).Take(limit);

        return (await query.ToListAsync(), count);
    }
}