using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models;

public class Mail
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Description { get; set; } = null!;

    [Required]
    public string Sender { get; set; } = null!;

    [Required]
    [RegularExpression(@"^[A-Za-z0-9\s]+str\.$")]
    public string Address { get; set; } = null!;
    
    [Required]
    [ForeignKey(nameof(Prisoner))]
    public int PrisonerId { get; set; }
    [Required]
    public virtual Prisoner Prisoner { get; set; } = null!;
}
