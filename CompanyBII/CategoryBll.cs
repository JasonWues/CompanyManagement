using Entity;
using ICompanyBll;
using ICompanyDal;

namespace CompanyBll;

public class CategoryBll : BaseBll<Category>,ICategoryBll
{
    public CategoryBll(ICategoryDal iCategory)
    {
        _iBaseDal = iCategory;
    }

    public async Task<(List<Category> list,int Count)> Query(string categoryName, int page, int limit)
    {
        var Categor = await _iBaseDal.Query();
        int count = 0;

        if (!string.IsNullOrEmpty(categoryName))
        {
            Categor = Categor.Where(x => x.CategoryName.Contains(categoryName)).ToList();
            count = Categor.Count();
        }

        count = Categor.Count();

        Categor = Categor.OrderBy(x => x.CategoryName).Skip((page-1) * 5).Take(limit).ToList();

        return (Categor, count);
    }
}