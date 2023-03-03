using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data;

public class StudentSystemContext : DbContext
{
	public StudentSystemContext()
	{
	}

	public StudentSystemContext(DbContextOptions<StudentSystemContext> options) : base(options)
	{
	}

	public virtual DbSet<Course> Courses { get; set; } = null!;
	public virtual DbSet<Homework> Homeworks { get; set; } = null!;
	public virtual DbSet<Resource> Resources { get; set; } = null!;
	public virtual DbSet<Student> Students { get; set; } = null!;
	public virtual DbSet<StudentCourse> StudentsCourses { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=StudentSystem;Integrated Security=True;TrustServerCertificate=true");
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Course>(entity =>
		{
			entity.HasKey(pk => pk.CourseId);

			entity.Property(e => e.CourseId);

			entity.Property(e => e.Name).HasMaxLength(80).IsUnicode();

			entity.Property(e => e.Description).HasMaxLength(100).IsUnicode();

			entity.Property(e => e.StartDate);

			entity.Property(e => e.EndDate);

			entity.Property(e => e.Price);
		});

        modelBuilder.Entity<Homework>(entity =>
        {
            entity.HasKey(pk => pk.HomeworkId);

            entity.Property(e => e.HomeworkId);

			entity.Property(e => e.Content).IsUnicode(false);

			entity.Property(e => e.ContentType);

			entity.Property(e => e.SubmissionTime);

			entity.HasOne(s => s.Student).WithMany(h => h.Homeworks).HasForeignKey(s => s.StudentId);

			entity.HasOne(c => c.Course).WithMany(h => h.Homeworks).HasForeignKey(c => c.CourseId);
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(pk => pk.ResourceId);

            entity.Property(e => e.ResourceId);

            entity.Property(e => e.Name).HasMaxLength(50).IsUnicode();

			entity.Property(e => e.Url).IsUnicode(false);

			entity.Property(e => e.ResourceType);

			entity.HasOne(c => c.Course).WithMany(r => r.Resources).HasForeignKey(c => c.CourseId);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(pk => pk.StudentId);

            entity.Property(e => e.StudentId);

            entity.Property(e => e.Name).HasMaxLength(100).IsUnicode();

            entity.Property(e => e.PhoneNumber).HasColumnType("varchar(10)").IsFixedLength();

            entity.Property(e => e.RegisteredOn);

            entity.Property(e => e.Birthday);
        });

        modelBuilder.Entity<StudentCourse>(entity =>
        {
            entity.HasKey(pk => new { pk.StudentId, pk.CourseId });

            entity
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentsCourses)
                .HasForeignKey(sc => sc.CourseId);

            entity
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentsCourses)
                .HasForeignKey(sc => sc.StudentId);
        });
    }
}
