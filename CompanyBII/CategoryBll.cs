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
}