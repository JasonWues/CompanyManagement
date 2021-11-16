using Entity;
using Entity.DTO;
using ICompanyBll;
using ICompanyDal;
using Microsoft.EntityFrameworkCore;
using Utility.ExtendMethod;

namespace CompanyBll
{
    public class UserInfoBll : BaseBll<UserInfo>, IUserInfoBll
    {
        readonly IDepartmentInfoDal _departmentInfoDal;
        public UserInfoBll(IUserInfoDal iUserInfoDal, IDepartmentInfoDal departmentInfoDal)
        {
            _iBaseDal = iUserInfoDal;
            _departmentInfoDal = departmentInfoDal;
        }

        public async Task<bool> Update(string Id, string userName, int sex, string phoneNum, string departmentInfoId)
        {
            UserInfo userInfo = await _iBaseDal.Find(Id);
            userInfo.UserName = userName;
            userInfo.Sex = sex;
            userInfo.PhoneNum = phoneNum;
            userInfo.DepartmentId = departmentInfoId;
            return await _iBaseDal.Update(userInfo);
        }

        public async Task<(List<UserInfo_DepartmentInfo> list, int count)> Query(string userName, string phoneNum, int page, int limit)
        {
            var userinfo = _iBaseDal.QueryDb().Where(x => x.IsDelete == false);

            var count = 0;
            if (!string.IsNullOrEmpty(userName))
            {
                userinfo = _iBaseDal.QueryDb().Where(x => x.UserName.Contains(userName) && x.IsDelete == false);
                count = userinfo.Count();
            }

            if (!string.IsNullOrEmpty(phoneNum))
            {
                userinfo = _iBaseDal.QueryDb().Where(x => x.PhoneNum.Contains(phoneNum) && x.IsDelete == false);
                count = userinfo.Count();
            }

            var departmentInfo = _departmentInfoDal.QueryDb();

            var query = from x in userinfo
                        join d in departmentInfo
                        on x.DepartmentId equals d.Id into grouping
                        from p in grouping.DefaultIfEmpty()
                        select new UserInfo_DepartmentInfo
                        {
                            UserName = x.UserName,
                            Id = x.Id,
                            Account = x.Account,
                            PhoneNum = x.PhoneNum,
                            Email = x.Email,
                            CreateTime = x.CreateTime.ToString("f"),
                            Sex = x.Sex.ConvertSex(),
                            DepartmentName = p.DepartmentName,
                            isAdmin = x.isAdmin.ConvertYesOrNo()
                        };

            count = query.Count();

            query = query.OrderBy(x => x.Id).Skip((page - 1) * limit).Take(limit);

            return (await query.ToListAsync(), count);
        }
    }
}
