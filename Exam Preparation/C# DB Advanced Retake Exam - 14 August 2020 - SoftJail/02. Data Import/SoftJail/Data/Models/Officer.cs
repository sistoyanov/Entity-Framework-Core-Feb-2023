using SoftJail.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models;

public class Officer
{
    public Officer()
    {
        this.OfficerPrisoners = new HashSet<OfficerPrisoner>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 3)]
    public string FullName { get; set; } = null!;

    [Required]
    //[Range(0, double.MaxValue)]
    public decimal Salary { get; set; }
    
    [Required]
    public Position Position { get; set; }
    
    [Required]
    public Weapon Weapon { get; set; }
    
    [Required]
    [ForeignKey(nameof(Department))]
    public int DepartmentId { get; set;}
    [Required]
    public virtual Department Department { get; set;} = null!;

    public virtual ICollection<OfficerPrisoner> OfficerPrisoners { get; set; } = null!;
}
