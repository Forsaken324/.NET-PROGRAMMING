namespace ContosoUniversity.Contracts;

public class CourseDto
{
    public int CourseID {get; set;}
    public required string Title {get; set;}
    public required int Credits {get; set;}
    
}