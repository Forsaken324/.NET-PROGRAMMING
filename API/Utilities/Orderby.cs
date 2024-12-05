using ContosoUniversity.Models;
namespace ContosoUniversity.Utilities;

public static class Ordering
{
    public static IQueryable<Student> OrderStudents(string? order, IQueryable<Student> students)
    {
        switch(order?.ToLower())
            {
                case "name_desc":
                    // returns students based on their lastname in ascending order
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                
                case "date":
                    // returns students based on their lastname in descending order 
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;

                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;

                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

        return students;
    }

    public static IQueryable<Course> OrderCourse(string? order, IQueryable<Course> courses)
    {
        switch(order?.ToLower())
        {
            case "title":
                courses = courses.OrderBy(c => c.Title);
                break;
            case "title_desc":
                courses = courses.OrderByDescending(c => c.Title);
                break;
            case "credit":
                courses = courses.OrderBy(c => c.Credits);
                break;
            case "credit_desc":
                courses = courses.OrderByDescending(c => c.Credits);
                break;
            default:
                courses = courses.OrderBy(c => c.Title);
                break;
        }
        return courses;
    }

}