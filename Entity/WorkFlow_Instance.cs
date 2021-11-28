using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class WorkFlow_Instance : BaseId
    {
        /// <summary>
        /// 工作流模板ID
        /// </summary>
        [Column(TypeName = "nvarchar(36)")]
        public string ModelId { get; set; } = null!;
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        [Column(TypeName = "nvarchar(64)")]
        public string? Description { get; set; }

        /// <summary>
        /// 申请理由
        /// </summary>
        [Column(TypeName = "nvarchar(64)")]
        public string? Reason { get; set; }
        public DateTime CreateTime { get; set; }

        [Column(TypeName = "nvarchar(36)")]
        public string Creator { get; set; } = null!;
        /// <summary>
        /// 出库数量
        /// </summary>
        public int OutNum { get; set; }
        /// <summary>
        /// 出库物资Id
        /// </summary>
        [Column(TypeName = "varchar(36)")]
        public string OutGoodsId { get; set; } = null!;
    }
}
