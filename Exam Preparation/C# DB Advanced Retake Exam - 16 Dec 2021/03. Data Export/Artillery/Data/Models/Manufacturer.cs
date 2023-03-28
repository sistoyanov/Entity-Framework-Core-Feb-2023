﻿using System.ComponentModel.DataAnnotations;

namespace Artillery.Data.Models;

public class Manufacturer
{
    public Manufacturer()
    {
        this.Guns = new HashSet<Gun>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    //[Index(IsUnique = true)]
    [StringLength(40, MinimumLength = 4)]
    public string ManufacturerName { get; set; } = null!;
    
    [Required]
    [StringLength(100, MinimumLength = 10)]
    public string Founded { get; set;} = null!;

    public virtual ICollection<Gun> Guns { get; set; } = null!;
}
