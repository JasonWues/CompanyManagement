using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class RoleInfo : BaseId
    {
        [Column(TypeName = "varchar(16)")]
        [Required]
        public string RoleName { get; set; } = null!;

        [Column(TypeName = "varchar(36)")]
        public string Description { get; set; } = null!;

        public DateTime CreateTime { get; set; }

        public bool IsDelete { get; set; }

        public DateTime DeleteTime { get; set; }
    }
}
