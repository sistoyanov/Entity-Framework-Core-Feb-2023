using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaporStore.Data.Models; 

public class GameTag 
{
    [Required]
    [ForeignKey(nameof(Game))]
    public int GameId { get; set; }
    [Required]
    public virtual Game Game { get; set; } = null!;

    [Required]
    public int TagId { get; set; }
    [Required]
    public virtual Tag Tag { get; set; } = null!;
}
