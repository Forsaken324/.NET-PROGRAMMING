using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.Controllers;

[ApiController]
[Route("api/{controller}")]

public class CourseController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CourseController(ApplicationDbContext context)
    {
        _context = context;
    }

    // [HttpGet]
    // public async Task<ActionResult<Course>>
}