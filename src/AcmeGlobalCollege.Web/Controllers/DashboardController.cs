using System.Security.Claims;
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

        // Redirects each logged-in user to the correct dashboard.
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

        // Shows summary data for the administrator dashboard.
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

        // Shows only the courses and students linked to the logged-in faculty user.
        [Authorize(Roles = "Faculty")]
        public async Task<IActionResult> Faculty()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            var facultyProfile = await _context.FacultyProfiles
                .FirstOrDefaultAsync(f => f.IdentityUserId == userId);

            if (facultyProfile == null)
            {
                return NotFound();
            }

            var courseAssignments = await _context.FacultyCourseAssignments
                .Include(fca => fca.Course)
                    .ThenInclude(c => c!.Branch)
                .Where(fca => fca.FacultyProfileId == facultyProfile.Id)
                .OrderBy(fca => fca.Course!.Name)
                .ToListAsync();

            var courseIds = courseAssignments
                .Select(fca => fca.CourseId)
                .Distinct()
                .ToList();

            var enrolments = await _context.CourseEnrolments
                .Include(e => e.StudentProfile)
                .Include(e => e.Course)
                    .ThenInclude(c => c!.Branch)
                .Where(e => courseIds.Contains(e.CourseId))
                .OrderBy(e => e.Course!.Name)
                .ThenBy(e => e.StudentProfile!.LastName)
                .ThenBy(e => e.StudentProfile!.FirstName)
                .ToListAsync();

            var viewModel = new FacultyDashboardViewModel
            {
                FacultyName = $"{facultyProfile.FirstName} {facultyProfile.LastName}",
                CourseAssignments = courseAssignments,
                Enrolments = enrolments
            };

            return View(viewModel);
        }

        // Shows the current student's own profile and enrolments.
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Student()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            var studentProfile = await _context.StudentProfiles
                .FirstOrDefaultAsync(s => s.IdentityUserId == userId);

            if (studentProfile == null)
            {
                return NotFound();
            }

            var enrolments = await _context.CourseEnrolments
                .Include(e => e.Course)
                    .ThenInclude(c => c!.Branch)
                .Include(e => e.Course)
                    .ThenInclude(c => c!.Modules) // 👈 ADD THIS
                .Where(e => e.StudentProfileId == studentProfile.Id)
                .OrderByDescending(e => e.EnrolDate)
                .ToListAsync();

            var viewModel = new StudentDashboardViewModel
            {
                Student = studentProfile,
                Enrolments = enrolments
            };

            return View(viewModel);
        }
    }
}