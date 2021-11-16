using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class R_UserInfo_RoleInfo : BaseId
    {
        [Column(TypeName = "nvarchar(36)")]
        [Required]
        public string UserId { get; set; } = null!;

        [Column(TypeName = "nvarchar(36)")]
        [Required]
        public string RoleId { get; set; } = null!;

        public DateTime CreateTime { get; set; }
    }
}
