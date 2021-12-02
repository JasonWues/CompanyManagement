
namespace Entity.DTO
{
    public class DepartmentInfo_UserInfo
    {
        public string Id { get; set; }
        public string? DepartmentName { get; set; }

        public string LeaderName { get; set; } = null!;

        public string ParentName { get; set; } = null!;

        public string CreateTime { get; set; } = null!;
    }
}
