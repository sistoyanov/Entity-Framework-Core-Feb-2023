﻿using System.ComponentModel.DataAnnotations;

namespace Boardgames.Data.Models;

public class Creator
{
    public Creator()
    {
        this.Boardgames = new HashSet<Boardgame>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(7, MinimumLength = 2)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(7, MinimumLength = 2)]
    public string LastName { get; set; } = null!;

    public virtual ICollection<Boardgame> Boardgames { get; set; } = null!;
}
