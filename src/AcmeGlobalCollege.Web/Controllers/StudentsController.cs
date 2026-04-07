using System.Security.Claims;
using AcmeGlobalCollege.Web.Data;
using AcmeGlobalCollege.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcmeGlobalCollege.Web.Controllers
{
    [Authorize(Roles = "Admin,Faculty")]
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lists students based on the current user's role.
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var allStudents = await _context.StudentProfiles
                    .OrderBy(s => s.LastName)
                    .ThenBy(s => s.FirstName)
                    .ToListAsync();

                return View(allStudents);
            }

            if (User.IsInRole("Faculty"))
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

                var assignedCourseIds = await _context.FacultyCourseAssignments
                    .Where(fca => fca.FacultyProfileId == facultyProfile.Id)
                    .Select(fca => fca.CourseId)
                    .Distinct()
                    .ToListAsync();

                var students = await _context.CourseEnrolments
                    .Include(e => e.StudentProfile)
                    .Where(e => assignedCourseIds.Contains(e.CourseId))
                    .Select(e => e.StudentProfile!)
                    .Distinct()
                    .OrderBy(s => s.LastName)
                    .ThenBy(s => s.FirstName)
                    .ToListAsync();

                return View(students);
            }

            return Forbid();
        }

        // Shows student details only when the current user is allowed to access them.
        public async Task<IActionResult> Details(int id)
        {
            var student = await _context.StudentProfiles
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Faculty"))
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

                var isLinkedStudent = await _context.CourseEnrolments
                    .AnyAsync(e =>
                        e.StudentProfileId == id &&
                        _context.FacultyCourseAssignments.Any(fca =>
                            fca.FacultyProfileId == facultyProfile.Id &&
                            fca.CourseId == e.CourseId));

                if (!isLinkedStudent)
                {
                    return Forbid();
                }
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