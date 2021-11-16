using Entity;
using ICompanyDal;

namespace CompanyDal;

public class MenuInfoDal : BaseDal<MenuInfo>,IMenuInfoDal
{
    readonly CompanyContext _companyContext;
    public MenuInfoDal(CompanyContext companyContext) : base(companyContext)
    {
        _companyContext = companyContext;
    }
}