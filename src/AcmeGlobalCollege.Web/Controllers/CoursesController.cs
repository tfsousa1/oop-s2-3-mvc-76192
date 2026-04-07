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

        // Lists all courses for the administrator.
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .Include(c => c.Branch)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(courses);
        }

        // Shows one course with branch, modules and linked enrolments.
        public async Task<IActionResult> Details(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Branch)
                .Include(c => c.Modules)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            var enrolments = await _context.CourseEnrolments
                .Include(e => e.StudentProfile)
                .Where(e => e.CourseId == id)
                .OrderBy(e => e.StudentProfile!.LastName)
                .ThenBy(e => e.StudentProfile!.FirstName)
                .ToListAsync();

            var viewModel = new CourseDetailsViewModel
            {
                Course = course,
                Modules = course.Modules.OrderBy(m => m.Name).ToList(),
                Enrolments = enrolments
            };

            return View(viewModel);
        }
    }
}