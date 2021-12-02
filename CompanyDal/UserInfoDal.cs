using Entity;
using ICompanyDal;

namespace CompanyDal
{
    public class UserInfoDal : BaseDal<UserInfo>, IUserInfoDal
    {
        readonly CompanyContext _companyContext;

        public UserInfoDal(CompanyContext companyContext) : base(companyContext)
        {
            _companyContext = companyContext;

        }
    }
}
