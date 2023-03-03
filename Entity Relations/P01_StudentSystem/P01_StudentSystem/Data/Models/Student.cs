namespace P01_StudentSystem.Data.Models;

public class Student
{
    public Student()
    {
        StudentsCourses = new HashSet<StudentCourse>();
        Homeworks = new HashSet<Homework>();
    }

    public int StudentId { get; set; }

    public string Name { get; set; } = null!;

    public string? PhoneNumber { get; set; } = null!;

    public DateTime RegisteredOn { get; set; }

    public DateTime? Birthday { get; set; } = null!;

    public virtual ICollection<StudentCourse> StudentsCourses { get; set; }

    public virtual ICollection<Homework> Homeworks { get; set; }
}
