
namespace Entity.DTO
{
    public class UserInfo_DepartmentInfo
    {
        public string Id { get; set; }
        public string Account { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string? PhoneNum { get; set; }

        public string? Email { get; set; }

        public string? DepartmentName { get; set; }

        public string Sex { get; set; } = null!;

        public string PassWord { get; set; } = null!;

        public string CreateTime { get; set; } = null!;

        public string? isAdmin { get; set; }
    }
}
