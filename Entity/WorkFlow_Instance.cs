using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class WorkFlow_Instance : BaseId
    {
        public int ModelId { get; set; }
        public int Status { get; set; }
        [Column(TypeName = "nvarchar(64)")]
        public string? Description { get; set; }
        [Column(TypeName = "nvarchar(64)")]
        public string? Reason { get; set; }
        public DateTime CreateTime { get; set; }

        [Column(TypeName = "nvarchar(36)")]
        public string Creator { get; set; }
        public int OutNum { get; set; }

        [Column(TypeName = "varchar(36)")]
        public string OutGoodsId { get; set; }
    }
}
