using Entity;
using ICompanyDal;

namespace CompanyDal;

public class CategoryDal : BaseDal<Category>, ICategoryDal
{
    private readonly CompanyContext _companyContext;
    public CategoryDal(CompanyContext companyContext) : base(companyContext)
    {
        _companyContext = companyContext;
    }
}