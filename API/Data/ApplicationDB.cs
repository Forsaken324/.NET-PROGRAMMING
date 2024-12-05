using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) {}

    public DbSet<Student> Students {get; set;}
    public DbSet<Enrollment> Enrollments {get; set;}
    public DbSet<Course> Courses {get; set;}
    public DbSet<Lecturer> Lecturers {get; set;}
    public DbSet<LecturerCourses> lecturerCourses {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>().ToTable("Course");
        modelBuilder.Entity<Student>().ToTable("Student");
        modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
    }

}