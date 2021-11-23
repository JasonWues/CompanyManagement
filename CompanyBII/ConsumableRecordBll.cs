using Entity;
using Entity.DTO;
using ICompanyBll;
using ICompanyDal;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace CompanyBll;

public class ConsumableRecordBll : BaseBll<ConsumableRecord>, IConsumableRecordBll
{
    readonly IUserInfoDal _iUserInfoDal;
    readonly IConsumableInfoDal _iConsumableInfoDal;
    public ConsumableRecordBll(IConsumableRecordDal iConsumableRecordDal, IUserInfoDal iUserInfoDal, IConsumableInfoDal iConsumableInfoDal)
    {
        _iBaseDal = iConsumableRecordDal;
        _iUserInfoDal = iUserInfoDal;
        _iConsumableInfoDal = iConsumableInfoDal;
    }

    public async Task<Stream> DownLoad()
    {
        var query = await (from x in _iBaseDal.QueryDb().AsQueryable()
                     join c in _iConsumableInfoDal.QueryDb().Where(x => x.IsDelete == false)
                     on x.ConsumableId equals c.Id
                     select new
                     {
                         x.Num,
                         c.Name,
                         CreateTime =  x.CreateTime.ToString("g"),
                         Type =  x.Type == 1 ? "入库" : "出库"
                     }).ToListAsync();

        var currentPath = Directory.GetCurrentDirectory();
        var fileName = "output.xlsx";
        var filePath = Path.Combine(currentPath, fileName);

        FileStream fileStream = new FileStream(filePath,FileMode.OpenOrCreate,FileAccess.ReadWrite);

        fileStream.Dispose();

        FileInfo fileInfo = new FileInfo(filePath);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var packapge = new ExcelPackage(fileInfo))
        {
            var worksheet = packapge.Workbook.Worksheets.Add("Sheet1");

            worksheet.Cells.LoadFromCollection(query, false);

            await packapge.SaveAsync();
        }

        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return fs;
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

    public async Task<(bool isAdd, string msg)> UpLoad(Stream stream, string userinfoId)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var package = new ExcelPackage(stream))
        {
            // 获取Exel指定工作簿，"Sheet1"也可以用索引代替
            ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];
            //全部行数
            int rowNum = worksheet.Dimension.Rows;
            //没有错误的实体数组
            List<ConsumableRecord> consumableRecords = new List<ConsumableRecord>();
            //修改
            List<ConsumableInfo> consumableInfos = new List<ConsumableInfo>();

            string errorMsg = string.Empty;

            for (int row = 2; row <= rowNum; row++)
            {
                if (worksheet.Cells[row, 1].Value == null)
                {
                    continue;
                }
                var name = worksheet.Cells[row, 1].Value.ToString().Trim();
                var consumableInfo = _iConsumableInfoDal.QueryDb().FirstOrDefault(c => c.Name == name);
                if (consumableInfo != null)
                {
                    consumableRecords.Add(new ConsumableRecord
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreateTime = DateTime.Now,
                        Type = 1,
                        Creator = userinfoId,
                        Num = int.Parse(worksheet.Cells[row, 2].Value.ToString()),
                        ConsumableId = consumableInfo.Id
                    });
                    await _iConsumableInfoDal.Update(x => x.Id == consumableInfo.Id, x => new ConsumableInfo { Num = x.Num + int.Parse(worksheet.Cells[row, 2].Value.ToString()) });
                }
                else
                {
                    errorMsg = string.Format("{0}商品名称有误,位于第{1}行", name, row);
                    return (false, errorMsg);
                }

            }

            await _iBaseDal.BatchInsert(consumableRecords);
            return (true, string.Empty);

        }
    }
}