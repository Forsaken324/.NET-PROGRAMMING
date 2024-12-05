using ContosoUniversity.Models;

namespace ContosoUniversity.Contracts;

public class StudentDto
{
    public int StudentID {get; set;}
    public required string LastName {get; set;}
    public required string FirstMidName {get; set;}

    public ICollection<Enrollment>? Enrollments {get; set;}
    
}