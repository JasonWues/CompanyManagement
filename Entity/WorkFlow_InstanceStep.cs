using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class WorkFlow_InstanceStep : BaseId
    {

        [Column(TypeName = "varchar(36)")]
        public string InstanceId { get; set; }
        [Column(TypeName = "varchar(36)")]
        public string ReviewerId { get; set; }
        [Column(TypeName = "nvarchar(64)")]
        public string? ReviewReason { get; set; }
        public int ReviewStatus { get; set; }
        public DateTime ReviewTime { get; set; }
        [Column(TypeName = "varchar(36)")]
        public string BeforeStepId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
