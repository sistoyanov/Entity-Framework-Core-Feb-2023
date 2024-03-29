﻿using System.Resources;

namespace P01_StudentSystem.Data.Models;

public class Course
{
    public Course()
    {
        StudentsCourses = new HashSet<StudentCourse>();
        Resources = new HashSet<Resource>();
        Homeworks = new HashSet<Homework>();
    }

    public int CourseId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<StudentCourse> StudentsCourses { get; set; }

    public virtual ICollection<Resource> Resources { get; set; }

    public virtual ICollection<Homework> Homeworks { get; set; }


}
