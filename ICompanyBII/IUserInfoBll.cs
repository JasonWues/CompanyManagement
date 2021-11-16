using Entity;
using Entity.DTO;

namespace ICompanyBll
{
    public interface IUserInfoBll : IBaseBll<UserInfo>
    {
        public Task<bool> Update(string Id,string userName,int sex,string phoneNum,string departmentInfoId);
        
        public Task<(List<UserInfo_DepartmentInfo> list,int count)> Query(string userName, string phoneNum, int page, int limit);
    }
}
