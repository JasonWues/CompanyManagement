using Entity;

namespace ICompanyBll
{
    public interface IRoleInfoBll : IBaseBll<RoleInfo>
    {
        public Task<bool> Update(string Id, string rolename, string desctiption);

        public Task<bool> FakeDelete(string Id);
    }
}
