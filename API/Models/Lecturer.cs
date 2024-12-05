namespace ContosoUniversity.Models;

public class Lecturer
{
    public int LecturerID {get; set;}
    public required string FirstMidName {get; set;}
    public required string LastName {get; set;}

    public ICollection<LecturerCourses>? MyCourses {get; set;} = new List<LecturerCourses>();
}