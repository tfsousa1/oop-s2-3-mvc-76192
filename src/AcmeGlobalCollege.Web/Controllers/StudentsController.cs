using AcmeGlobalCollege.Web.Data;
using AcmeGlobalCollege.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcmeGlobalCollege.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _context.StudentProfiles
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToListAsync();

            return View(students);
        }

        public async Task<IActionResult> Details(int id)
        {
            var student = await _context.StudentProfiles
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            var enrolments = await _context.CourseEnrolments
                .Include(e => e.Course)
                .ThenInclude(c => c!.Branch)
                .Where(e => e.StudentProfileId == id)
                .OrderByDescending(e => e.EnrolDate)
                .ToListAsync();

            var viewModel = new StudentDetailsViewModel
            {
                Student = student,
                Enrolments = enrolments
            };

            return View(viewModel);
        }
    }
}