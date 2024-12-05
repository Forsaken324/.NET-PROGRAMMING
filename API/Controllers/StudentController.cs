using Microsoft.AspNetCore.Mvc;
using ContosoUniversity.Contracts;
using ContosoUniversity.Models;
using ContosoUniversity.Data;
using ContosoUniversity.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;

namespace ContosoUniversity.Controllers;

[ApiController]
[Route("api/{controller}")]

public class StudentController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StudentController(ApplicationDbContext context)
    {
        _context = context;
    }

    private static StudentDto StudentDtoConverter(Student student) => 
        new StudentDto
        {
            StudentID = student.StudentID,
            LastName = student.LastName,
            FirstMidName = student.FirstMidName,
            Enrollments = student.Enrollments
        };

    // checks if a student exists in the database
    private bool StudentExits(int id)
    {
        return _context.Students.Any(s => s.StudentID == id);
    }
    // checks url and determines if pagination is necessary
    private bool ShouldParginate(bool? paginate, int? pageIndex, int? pageSize)
    {
        return paginate == true && pageIndex is not null && pageSize is not null && pageSize > 0;
    }

    
    
    

    // Getting a student either generally or specific using an id
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudent(int? id, string? order, string? searchString, bool? paginate, int? pageIndex, int? pageSize)
    {
        // searches for a single student and returns the result
       if(id is not null)
       {
            
            var student = await _context.Students
                .Include(e => e.Enrollments)
                .ThenInclude(c => c.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.StudentID == id);
            if(student is null)
            {
                return NotFound();
            }
            return Ok(StudentDtoConverter(student));

                
       };

// a method then pass in the order value and the querryable to order. so what will now be the return type of that order?
// the method is in the utilities folder

// asks first if should paginate a searched string before paginating
       if(ShouldParginate(paginate, pageIndex, pageSize))
       {
            if(!String.IsNullOrEmpty(searchString))
            {
                if(searchString == " ") return BadRequest();
                var searchedStudents = _context.Students.Where(s => s.FirstMidName.ToUpper().Contains(searchString.ToUpper()) ||
                     s.LastName.ToUpper().Contains(searchString.ToUpper()));
                
                
                if(order is not null)
                {
                    searchedStudents = Ordering.OrderStudents(order, searchedStudents);
                }
                var searchedStudentsToDto = searchedStudents.Select(student => StudentDtoConverter(student));
                
                if(searchedStudentsToDto is null)
                {
                    return NotFound();
                }
                
                var paginatedList = await PaginatedList<StudentDto>.CreateAsync(searchedStudentsToDto, (int)pageIndex, (int)pageSize);
                return Ok(
                    new {
                        Data = paginatedList,
                        Pagination = new 
                            {
                                paginatedList.PageIndex,
                                paginatedList.TotalPages,
                                paginatedList.HasPreviousPage,
                                paginatedList.HasNextPage
                            }
                    }
                );
            }

            // paginates regardless of search string
            var paginatedStudent = from s in _context.Students select s;
            // paginates according to order if required
            if(order is not null)
            {
                paginatedStudent = Ordering.OrderStudents(order, paginatedStudent);
            }

            var source = paginatedStudent.Select(student => StudentDtoConverter(student));
            var allStudentPaginatedList = await PaginatedList<StudentDto>.CreateAsync(source, (int)pageIndex, (int)pageSize);

            return Ok(
                new {
                    Data = allStudentPaginatedList,
                    pagination = new {
                        allStudentPaginatedList.PageIndex,
                        allStudentPaginatedList.TotalPages,
                        allStudentPaginatedList.HasPreviousPage,
                        allStudentPaginatedList.HasNextPage
                    }
                }
            );
       }

        // returns results without paginating
       var students =   from s in _context.Students select s;
        // Orders the results to be returned if necessary
       if(order is not null)
       {
        students = Ordering.OrderStudents(order, students);
       }
       var studentDto = students.Select(student => StudentDtoConverter(student));
       var result = await studentDto.ToListAsync();

       return Ok(result);

        
    }

    // creating a student

    [HttpPost]
    public async Task<IActionResult> PostStudent(Student student)
    {
        
        if(student is null)
        {
            return NotFound();
        }

        try
        {
            if(ModelState.IsValid)
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(PostStudent), new {id = student.StudentID}, student);
            }
        }
        catch(DbUpdateException ex)
        {
            Console.WriteLine(ex.Message);
            ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, if the problem persists " +
                "see your system administrator.");
        }

        return StatusCode(500);
    }

    // updating student details in the database

    [HttpPut]
    public async Task<IActionResult> PutStudent(int id, [FromBody] Student student)
    {
        if(id != student.StudentID || student is null)
        {
            Console.WriteLine("Error is from here");
            return BadRequest();
        }
        var studentToUpdate = await _context.Students.FirstOrDefaultAsync(s => s.StudentID == id);
        Console.WriteLine("student has been found");

        if(studentToUpdate is null)
        {
            Console.WriteLine("This block has been reached");
            return NotFound();
        }

        
        studentToUpdate.LastName = student.LastName;
        studentToUpdate.FirstMidName = student.FirstMidName;
        studentToUpdate.EnrollmentDate = student.EnrollmentDate;

        
        try
        {
            await _context.SaveChangesAsync();
            Console.WriteLine("Here has been reached");
            return NoContent();
        }
        catch(DbUpdateException ex)
        {
            Console.WriteLine(ex.Message);
            ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, if the problem persists " +
                "see your system administrator.");
        }
        
        
        return StatusCode(500);

  
    }

    // Deleting a student from the database
    [HttpDelete]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var studentToDelete = await _context.Students.FirstOrDefaultAsync(s => s.StudentID == id);

        if(studentToDelete == null)
        {
            return BadRequest();
        }

        try
        {
            _context.Students.Remove(studentToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch(DbUpdateException ex)
        {
            Console.WriteLine(ex.Message);
            ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, if the problem persists " +
                "see your system administrator.");

        }

        return StatusCode(500);
    }
    
}