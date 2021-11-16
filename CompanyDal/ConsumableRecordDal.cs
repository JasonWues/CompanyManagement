using Entity;
using ICompanyDal;

namespace CompanyDal;

public class ConsumableRecordDal : BaseDal<ConsumableRecord>,IConsumableRecordDal
{
    readonly CompanyContext _companyContext;
    public ConsumableRecordDal(CompanyContext companyContext) : base(companyContext)
    {
        _companyContext = companyContext;
    }
}