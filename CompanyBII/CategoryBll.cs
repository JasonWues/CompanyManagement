using Entity;
using ICompanyBll;
using ICompanyDal;

namespace CompanyBll;

public class CategoryBll : BaseBll<Category>, ICategoryBll
{
    public CategoryBll(ICategoryDal iCategory)
    {
        _iBaseDal = iCategory;
    }

    public async Task<(List<Category> list, int Count)> Query(string categoryName, int page, int limit)
    {
        var categor = await _iBaseDal.Query();
        int count = 0;

        if (!string.IsNullOrEmpty(categoryName))
        {
            categor = categor.Where(x => x.CategoryName.Contains(categoryName)).ToList();
            count = categor.Count();
        }

        count = categor.Count();

        categor = categor.OrderBy(x => x.CategoryName).Skip((page - 1) * 5).Take(limit).ToList();

        return (categor, count);
    }

    public async Task<bool> Update(string Id, string categoryName, string description)
    {
        var categor = await _iBaseDal.Find(Id);
        categor.CategoryName = categoryName;
        categor.Description = description;
        return await _iBaseDal.Update(categor);
    }
}