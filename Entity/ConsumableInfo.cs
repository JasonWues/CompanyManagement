using System.ComponentModel.DataAnnotations.Schema;

namespace Entity;

public class ConsumableInfo : BaseId
{
    [Column(TypeName = "nvarchar(32)")]
    public string? Description { get; set; }

    [Column(TypeName = "varchar(36)")]
    public string CategoryId { get; set; }

    [Column(TypeName = "nvarchar(16)")]
    public string Name { get; set; }

    [Column(TypeName = "varchar(32)")]
    public string Specification { get; set; }

    public int Num { get; set; }

    [Column(TypeName = "nvarchar(8)")]
    public string Unit { get; set; }
    public decimal Money { get; set; }
    public int WarningNum { get; set; }
    public bool IsDelete { get; set; }
    public DateTime DeleteTime { get; set; }
    public DateTime CreateTime { get; set; }
}