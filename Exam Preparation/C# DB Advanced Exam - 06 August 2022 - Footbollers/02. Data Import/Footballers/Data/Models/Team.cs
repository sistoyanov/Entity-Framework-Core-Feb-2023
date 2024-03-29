﻿using System.ComponentModel.DataAnnotations;

namespace Footballers.Data.Models;

public class Team
{
    public Team()
    {
        this.TeamsFootballers = new HashSet<TeamFootballer>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(40)]
    [RegularExpression("^[A-Za-z0-9 .-]+$")]
    public string Name { get; set; } = null!;

    [Required]
    [MinLength(2)]
    [MaxLength(40)]
    public string Nationality { get; set; } = null!;
    
    [Required]
    public int Trophies { get; set; }

    public virtual ICollection<TeamFootballer> TeamsFootballers { get; set; } = null!;
}
