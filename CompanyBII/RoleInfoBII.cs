using Entity;
using ICompanyBll;
using ICompanyDal;

namespace CompanyBll
{
    public class RoleInfoBll : BaseBll<RoleInfo>, IRoleInfoBll
    {
        public RoleInfoBll(IRoleInfoDal iRoleInfoDal)
        {
            base._iBaseDal = iRoleInfoDal;
        }

        public async Task<bool> FakeDelete(string Id)
        {
            RoleInfo roleInfo = await _iBaseDal.Find(Id);
            roleInfo.IsDelete = true;
            roleInfo.DeleteTime = DateTime.Now;
            return await _iBaseDal.Update(roleInfo);
        }

        public async Task<bool> Update(string Id, string rolename, string desctiption)
        {
            RoleInfo roleInfo = await _iBaseDal.Find(Id);
            roleInfo.RoleName = rolename;
            roleInfo.Description = desctiption;
            return await _iBaseDal.Update(roleInfo);
        }
    }
}
