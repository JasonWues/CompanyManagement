using Entity;
using ICompanyDal;

namespace CompanyDal;

public class ConsumableInfoDal : BaseDal<ConsumableInfo>,IConsumableInfoDal
{
    readonly CompanyContext _companyContext;
    public ConsumableInfoDal(CompanyContext companyContext) : base(companyContext)
    {
        _companyContext = companyContext;
    }
}