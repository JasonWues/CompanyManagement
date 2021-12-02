using Entity;
using Entity.DTO;
using ICompanyBll;
using ICompanyDal;
using Microsoft.EntityFrameworkCore;

namespace CompanyBll;

public class ConsumableInfoBll : BaseBll<ConsumableInfo>, IConsumableInfoBll
{
    readonly ICategoryDal _iCategoryDal;
    public ConsumableInfoBll(IConsumableInfoDal iConsumableInfoDal, ICategoryDal iCategoryDal)
    {
        _iBaseDal = iConsumableInfoDal;
        _iCategoryDal = iCategoryDal;
    }

    public async Task<(List<ConsumableInfo_Category> list, int count)> Query(string name, int page, int limit)
    {
        var ConsumableInfo = _iBaseDal.QueryDb().Where(x => x.IsDelete == false);

        var count = 0;

        if (!string.IsNullOrEmpty(name))
        {
            ConsumableInfo = _iBaseDal.QueryDb().Where(x => x.Name.Contains(name) && x.IsDelete == false);
            count = ConsumableInfo.Count();
        }

        var query = from x in ConsumableInfo
                    join s in _iCategoryDal.QueryDb()
                    on x.CategoryId equals s.Id into grouping
                    from p in grouping.DefaultIfEmpty()
                    select new ConsumableInfo_Category
                    {
                        Id = x.Id,
                        Description = x.Description,
                        CategoryName = p.CategoryName,
                        Name = x.Name,
                        Specification = x.Specification,
                        Num = x.Num,
                        Unit = x.Unit,
                        Money = x.Money,
                        CreateTime = x.CreateTime.ToString("g"),
                    };
        count = query.Count();

        query = query.OrderBy(x => x.Name).Skip((page - 1) * limit).Take(limit);

        return (await query.ToListAsync(), count);

    }
}