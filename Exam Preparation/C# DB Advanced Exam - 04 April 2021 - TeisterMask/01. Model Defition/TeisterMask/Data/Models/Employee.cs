using System.ComponentModel.DataAnnotations;

namespace TeisterMask.Data.Models;

public class Employee
{
    public Employee()
    {
        this.EmployeesTasks = new HashSet<EmployeeTask>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(40, MinimumLength = 3)]
    //[RegularExpression(@"^[a-zA-Z0-9]*$")]
    public string Username { get; set; } = null!;

    [Required]
    //[EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    //[RegularExpression(@"^\d{3}-\d{3}-\d{4}$")]
    public string Phone { get; set; } = null!;

    public virtual ICollection<EmployeeTask> EmployeesTasks { get; set; } = null!;
}
