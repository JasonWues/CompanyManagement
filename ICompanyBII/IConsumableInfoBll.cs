using Entity;
using Entity.DTO;

namespace ICompanyBll;

public interface IConsumableInfoBll : IBaseBll<ConsumableInfo>
{
    public Task<(List<ConsumableInfo_Category> list, int count)> Query(string name, int page, int limit);
}