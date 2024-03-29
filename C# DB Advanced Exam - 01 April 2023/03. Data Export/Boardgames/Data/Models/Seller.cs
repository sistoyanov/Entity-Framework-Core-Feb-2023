﻿using System.ComponentModel.DataAnnotations;

namespace Boardgames.Data.Models;

public class Seller
{
    public Seller()
    {
        this.BoardgamesSellers = new HashSet<BoardgameSeller>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 5)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(30, MinimumLength = 2)]
    public string Address { get; set; } = null!;

    [Required]
    public string Country { get; set; } = null!;

    [Required]
    [RegularExpression(@"^www\.[a-zA-Z0-9\-]+\.com$")]
    public string Website { get; set; } = null!;

    public virtual ICollection<BoardgameSeller> BoardgamesSellers { get; set; } = null!;
}
