using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity;

public class MenuInfo : BaseId
{
    [Column(TypeName = "nvarchar(16)")]
    [Required]
    public string Title { get; set; }= null!;
    
    [Column(TypeName = "nvarchar(16)")]
    public string? Description { get; set; }
    
    public int Level { get; set; }
    public int Sort { get; set; }
    
    [Column(TypeName = "varchar(128)")]
    public string? Href { get; set; }
        
    [Column(TypeName = "varchar(36)")]
    public string? ParentId { get; set; }
            
    [Column(TypeName = "varchar(32)")]
    public string? Icon { get; set; }
    
    [Column(TypeName = "varchar(16)")]
    public string? Target { get; set; }
    
    public DateTime CreateTime { get; set; }
    
    public bool IsDelete { get; set; }
    
    public DateTime DeleteTime { get; set; }
    
}