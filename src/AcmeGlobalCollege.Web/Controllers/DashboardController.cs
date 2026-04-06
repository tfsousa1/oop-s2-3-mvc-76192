using AcmeGlobalCollege.Web.Data;
using AcmeGlobalCollege.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcmeGlobalCollege.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(Admin));
            }

            if (User.IsInRole("Faculty"))
            {
                return RedirectToAction(nameof(Faculty));
            }

            if (User.IsInRole("Student"))
            {
                return RedirectToAction(nameof(Student));
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin()
        {
            var viewModel = new AdminDashboardViewModel
            {
                TotalBranches = await _context.Branches.CountAsync(),
                TotalCourses = await _context.Courses.CountAsync(),
                TotalFaculty = await _context.FacultyProfiles.CountAsync(),
                TotalStudents = await _context.StudentProfiles.CountAsync(),
                TotalEnrolments = await _context.CourseEnrolments.CountAsync(),
                Branches = await _context.Branches
                    .OrderBy(b => b.Name)
                    .ToListAsync(),
                Courses = await _context.Courses
                    .Include(c => c.Branch)
                    .OrderBy(c => c.Name)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Faculty")]
        public IActionResult Faculty()
        {
            return View();
        }

        [Authorize(Roles = "Student")]
        public IActionResult Student()
        {
            return View();
        }
    }
}