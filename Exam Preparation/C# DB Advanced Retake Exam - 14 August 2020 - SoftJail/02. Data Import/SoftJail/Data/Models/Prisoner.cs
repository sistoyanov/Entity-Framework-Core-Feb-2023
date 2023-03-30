using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models;

public class Prisoner
{
    public Prisoner()
    {
        this.Mails= new HashSet<Mail>();
        this.PrisonerOfficers = new HashSet<OfficerPrisoner>();
    }
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 3)]
    public string FullName { get; set; } = null!;

    [Required]
    //[RegularExpression(@"^The [A-Z][a-z]*$")]
    public string Nickname { get; set; } = null!;
    
    [Required]
    //[Range(18, 65)]
    public int Age { get; set; }
    
    [Required]
    public DateTime IncarcerationDate { get; set; }

    public DateTime? ReleaseDate { get; set; }

    //[Range(0, double.MaxValue)]
    public decimal? Bail { get; set; }

    [ForeignKey(nameof(Cell))]
    public int?	CellId { get; set; }
    public virtual Cell Cell { get; set; } = null!;

    public virtual ICollection<Mail> Mails { get; set; }

    public virtual ICollection<OfficerPrisoner> PrisonerOfficers { get; set; }

}
