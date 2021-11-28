using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity;

public class UserInfo : BaseId
{   
    [Column(TypeName = "varchar(16)")]
    [Required]
    public string Account { get; set; }= null!;
    
    [Column(TypeName = "nvarchar(16)")]
    [Required]
    public string UserName { get; set; }= null!;
    
    [Column(TypeName = "varchar(16)")]
    public string? PhoneNum { get; set; }
    
    [Column(TypeName = "varchar(32)")]
    public string? Email { get; set; }
    
    [Column(TypeName = "varchar(36)")]
    public string? DepartmentId { get; set; }
    
    [Required]
    public int Sex { get; set; }
    
    [Column(TypeName = "varchar(32)")]
    [Required]
    public string PassWord { get; set; }= null!;
    
    public DateTime CreateTime { get; set; }
    
    public bool IsDelete { get; set; }
    
    public DateTime? DeleteTime { get; set; }

    public int IsAdmin { get; set; }
}