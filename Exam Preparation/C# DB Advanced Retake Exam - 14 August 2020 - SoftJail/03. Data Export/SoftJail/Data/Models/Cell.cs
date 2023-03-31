﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    public class Cell
    {
        public Cell()
        {
            this.Prisoners= new HashSet<Prisoner>();
        }

        [Key]
        public int Id { get; set; }
        
        [Required]
        //[Range(1, 1000)]
        public int CellNumber { get; set; }
        
        [Required]
        public bool HasWindow { get; set; }
        
        [Required]
        [ForeignKey(nameof(Department))]
        public int DepartmentId { get; set; }
        [Required]
        public virtual Department Department { get; set; } = null!;

        public ICollection<Prisoner> Prisoners { get; set; } = null!;
    }
}