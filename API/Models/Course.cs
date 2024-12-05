using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models;

public class Course
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int CourseID {get; set;}
    public required string Title {get; set;}
    public required int Credits {get; set;}
    public ICollection<LecturerCourses> LecturerCourses {get; set;} = new List<LecturerCourses>();
    public ICollection<Enrollment>? Enrollments {get; set;}
}