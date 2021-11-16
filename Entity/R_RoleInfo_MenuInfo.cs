using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class R_RoleInfo_MenuInfo : BaseId
    {
        [Column(TypeName = "varchar(36)")]
        public string? RoleId { get; set; }
        [Column(TypeName = "varchar(36)")]
        public string? MenuId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
