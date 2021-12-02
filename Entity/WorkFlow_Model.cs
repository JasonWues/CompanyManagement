using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class WorkFlow_Model : BaseId
    {
        [Column(TypeName = "nvarchar(32)")]
        public string Title { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsDelete { get; set; }
        public DateTime DeleteTime { get; set; }
        [Column(TypeName = "varchar(64)")]
        public string? Description { get; set; }
    }
}
