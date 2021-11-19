using Entity;
using Entity.DTO;
using ICompanyBll;
using ICompanyDal;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace CompanyBll;

public class ConsumableRecordBll : BaseBll<ConsumableRecord>,IConsumableRecordBll
{
    readonly IUserInfoDal _iUserInfoDal;
    readonly IConsumableInfoDal _iConsumableInfoDal;
    public ConsumableRecordBll(IConsumableRecordDal iConsumableRecordDal, IUserInfoDal iUserInfoDal, IConsumableInfoDal iConsumableInfoDal)
    {
        _iBaseDal = iConsumableRecordDal;
        _iUserInfoDal = iUserInfoDal;
        _iConsumableInfoDal = iConsumableInfoDal;
    }

    public async Task<(List<Record_ConsumableInfo_UserInfo> list, int count)> Query(int page, int limit, string consumableId)
    {
        var consumableRecord = _iBaseDal.QueryDb().AsQueryable();
        var count = 0;
        if (!string.IsNullOrEmpty(consumableId))
        {
            consumableRecord = _iBaseDal.QueryDb().Where(x => x.ConsumableId == consumableId);
            count = consumableRecord.Count();
        }

        var userInfo = _iUserInfoDal.QueryDb().Where(x => x.IsDelete == false);
        var consumableInfo = _iConsumableInfoDal.QueryDb().Where(x => x.IsDelete == false);

        var query = from record in consumableRecord
                     join consumable in consumableInfo
                     on record.ConsumableId equals consumable.Id into record_consumable
                     from cc in record_consumable.DefaultIfEmpty()

                     join u in userInfo
                     on record.Creator equals u.Id into record_Userinfo
                     from ccu in record_Userinfo.DefaultIfEmpty()
                     select new Record_ConsumableInfo_UserInfo
                     {
                         Id = record.Id,
                         CreateTime = cc.CreateTime.ToString("f"),
                         Num = record.Num,
                         Type = record.Type == 1 ? "入库" : "出库",
                         ConsumableName = cc.Name,
                         UserName = ccu.UserName
                     };

        count = query.Count();

        query = query.OrderBy(x => x.UserName).Skip((page - 1) * limit).Take(limit);

        return (await query.ToListAsync(), count);
    }

    public async Task UpLoad(Stream stream, string userinfoId)
    {
        using (var package = new ExcelPackage(stream))
        {
            // 获取Exel指定工作簿，"Sheet1"也可以用索引代替
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];
            int RowNum = worksheet.Dimension.Rows;
            List<ConsumableRecord> consumableRecords = new List<ConsumableRecord>();


            for (int row = 1; row <= RowNum; row++)
            {
                var name = worksheet.Cells[row, 1].Value.ToString();
                var consumableInfo = _iConsumableInfoDal.QueryDb().FirstOrDefault(c => c.Name == name);

                consumableRecords.Add(new ConsumableRecord
                {
                    Id = Guid.NewGuid().ToString(),
                    CreateTime = DateTime.Now,
                    Type = 1,
                    Creator = userinfoId,
                    Num = int.Parse(worksheet.Cells[row, 2].Value.ToString()),
                    ConsumableId = consumableInfo != null ? consumableInfo.Id : ""
                });
            }

            await _iBaseDal.BatchInsert(consumableRecords);
        }
    }
}