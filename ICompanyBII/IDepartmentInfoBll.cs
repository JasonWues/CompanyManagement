using Entity;
using Entity.DTO;

namespace ICompanyBll
{
    public interface IDepartmentInfoBll : IBaseBll<DepartmentInfo>
    {
        public Task<bool> Update(string Id, string departmentName, string leaderId, string parentId, string description);

        public Task<bool> FakeDelete(string Id);

        public Task<(List<DepartmentInfo_UserInfo> list, int count)> Query(string departmentName, int page, int limit);
    }
}
