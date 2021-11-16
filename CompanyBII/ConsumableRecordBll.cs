using Entity;
using ICompanyBll;
using ICompanyDal;

namespace CompanyBll;

public class ConsumableRecordBll : BaseBll<ConsumableRecord>,IConsumableRecordBll
{
    public ConsumableRecordBll(IConsumableRecordDal iConsumableRecord)
    {
        _iBaseDal = iConsumableRecord;
    }
}