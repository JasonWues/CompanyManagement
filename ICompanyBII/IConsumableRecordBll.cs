using Entity;
using Entity.DTO;

namespace ICompanyBll;

public interface IConsumableRecordBll : IBaseBll<ConsumableRecord>
{
    public Task<(List<Record_ConsumableInfo_UserInfo> list, int count)> Query(int page, int limit, string consumableId);

    public Task<(bool isAdd,string msg)> UpLoad(Stream stream, string userinfoId);
}