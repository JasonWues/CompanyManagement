using Entity;
using Entity.DTO;
using ICompanyBll;
using ICompanyDal;
using Microsoft.EntityFrameworkCore;

namespace CompanyBll
{
    public class DepartmentInfoBll : BaseBll<DepartmentInfo>, IDepartmentInfoBll
    {
        readonly IUserInfoDal _userInfoDal;
        public DepartmentInfoBll(IDepartmentInfoDal idepartmentInfoDal, IUserInfoDal userInfoDal)
        {
            _iBaseDal = idepartmentInfoDal;
            _userInfoDal = userInfoDal;
        }

        public async Task<bool> FakeDelete(string Id)
        {
            DepartmentInfo departmentInfo = await _iBaseDal.Find(Id);
            departmentInfo.IsDelete = true;
            departmentInfo.DeleteTime = DateTime.Now;
            return await _iBaseDal.Update(departmentInfo);
        }

        public async Task<(List<DepartmentInfo_UserInfo> list, int count)> Query(string departmentName, int page, int limit)
        {
            var departmentInfo = _iBaseDal.QueryDb().Where(x => x.IsDelete == false);

            var count = 0;
            if (!string.IsNullOrEmpty(departmentName))
            {
                departmentInfo = _iBaseDal.QueryDb().Where(x => x.DepartmentName.Contains(departmentName) && x.IsDelete == false);
                count = departmentInfo.Count();
            }

            var userinfo = _userInfoDal.QueryDb();

            var query = from x in departmentInfo
                        join d in userinfo
                        on x.LeaderId equals d.Id into grouping
                        from p in grouping.DefaultIfEmpty()
                        join w in _iBaseDal.QueryDb().Where(x => x.IsDelete == false)
                        on x.ParentId equals w.Id into g
                        from k in g.DefaultIfEmpty()
                        select new DepartmentInfo_UserInfo
                        {
                            Id = x.Id,
                            DepartmentName = x.DepartmentName,
                            LeaderName = p.Account,
                            ParentName = k.DepartmentName,
                            CreateTime = x.CreateTime.ToString("f"),
                        };

            count = query.Count();

            query = query.OrderBy(x => x.Id).Skip((page - 1) * limit).Take(limit);

            return (await query.ToListAsync(), count);
        }

        public async Task<bool> Update(string Id, string departmentName, string leaderId, string parentId, string description)
        {
            DepartmentInfo departmentInfo = await _iBaseDal.Find(Id);
            departmentInfo.DepartmentName = departmentName;
            departmentInfo.LeaderId = leaderId;
            departmentInfo.ParentId = parentId;
            departmentInfo.Description = description;
            return await _iBaseDal.Update(departmentInfo);
        }
    }
}
