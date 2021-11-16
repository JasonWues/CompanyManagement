using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class DepartmentInfo : BaseId
    {
        [Column(TypeName = "nvarchar(16)")]
        [Required]
        public string DepartmentName { get; set; } = null!;
        
        [Column(TypeName = "nvarchar(32)")]
        public string Description { get; set; } = null!;

        [Column(TypeName = "varchar(36)")]
        [Required]
        public string LeaderId { get; set; } = null!;

        [Column(TypeName = "varchar(36)")]
        public string? ParentId { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsDelete { get; set; }

        public DateTime? DeleteTime { get; set; }
    }
}
