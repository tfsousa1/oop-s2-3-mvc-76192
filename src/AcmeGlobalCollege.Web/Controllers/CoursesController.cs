using AcmeGlobalCollege.Web.Data;
using AcmeGlobalCollege.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcmeGlobalCollege.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .Include(c => c.Branch)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(courses);
        }

        public async Task<IActionResult> Details(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Branch)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            var modules = await _context.Modules
                .Where(m => m.CourseId == id)
                .OrderBy(m => m.Name)
                .ToListAsync();

            var facultyAssignments = await _context.FacultyCourseAssignments
                .Include(fca => fca.FacultyProfile)
                .Where(fca => fca.CourseId == id)
                .OrderBy(fca => fca.FacultyProfile!.LastName)
                .ThenBy(fca => fca.FacultyProfile!.FirstName)
                .ToListAsync();

            var enrolments = await _context.CourseEnrolments
                .Include(e => e.StudentProfile)
                .Where(e => e.CourseId == id)
                .OrderBy(e => e.StudentProfile!.LastName)
                .ThenBy(e => e.StudentProfile!.FirstName)
                .ToListAsync();

            var viewModel = new CourseDetailsViewModel
            {
                Course = course,
                Modules = modules,
                FacultyAssignments = facultyAssignments,
                Enrolments = enrolments
            };

            return View(viewModel);
        }
    }
}