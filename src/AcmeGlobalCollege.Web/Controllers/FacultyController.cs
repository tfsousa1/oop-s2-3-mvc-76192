using AcmeGlobalCollege.Web.Data;
using AcmeGlobalCollege.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcmeGlobalCollege.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FacultyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FacultyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var facultyProfiles = await _context.FacultyProfiles
                .Include(f => f.FacultyCourseAssignments)
                    .ThenInclude(fca => fca.Course)
                .OrderBy(f => f.LastName)
                .ThenBy(f => f.FirstName)
                .ToListAsync();

            var viewModel = facultyProfiles.Select(f => new FacultyListItemViewModel
            {
                FacultyProfileId = f.Id,
                FullName = f.FullName,
                Email = f.Email,
                Phone = f.Phone,
                AssignedCourses = f.FacultyCourseAssignments
                    .Where(a => a.Course != null)
                    .Select(a => a.IsTutor
                        ? $"{a.Course!.Name} (Tutor)"
                        : a.Course!.Name)
                    .OrderBy(name => name)
                    .ToList()
            }).ToList();

            return View(viewModel);
        }
    }
}