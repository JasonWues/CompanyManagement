using System.ComponentModel.DataAnnotations.Schema;

namespace Entity;

public class Category : BaseId
{
    [Column(TypeName = "nvarchar(16)")]
    public string CategoryName { get; set; }

    [Column(TypeName = "nvarchar(32)")]
    public string? Description { get; set; }
}