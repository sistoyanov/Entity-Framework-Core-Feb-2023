﻿using System.ComponentModel.DataAnnotations;

namespace TeisterMask.Data.Models;

public class Project
{
    public Project()
    {
        this.Tasks= new HashSet<Task>();
    }
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(40, MinimumLength = 2)]
    public string Name { get; set; } = null!;
    
    [Required]
    public DateTime OpenDate { get; set; }

    public DateTime? DueDate { get; set; }

    public virtual ICollection<Task> Tasks { get; set; }
}
