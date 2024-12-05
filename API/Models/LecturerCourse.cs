namespace ContosoUniversity.Models;

public class LecturerCourses
{
    public int LecturerID {get; set;}
    public int CourseID {get; set;}

    public required Course Course {get; set;}
    public required Lecturer Lecturer {get; set;}
}