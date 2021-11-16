using System.ComponentModel.DataAnnotations.Schema;

namespace Entity;

public class ConsumableRecord : BaseId
{
    [Column(TypeName = "varchar(36)")]
    public string ConsumableId { get; set; }
    public int Num { get; set; }
    public int Type { get; set; }
    public DateTime CreateTime { get; set; }
    [Column(TypeName = "varchar(36)")]
    public string? Creator { get; set; }
}