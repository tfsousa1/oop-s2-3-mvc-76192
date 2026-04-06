using AcmeGlobalCollege.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcmeGlobalCollege.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EnrolmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrolmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var enrolments = await _context.CourseEnrolments
                .Include(e => e.StudentProfile)
                .Include(e => e.Course)
                    .ThenInclude(c => c!.Branch)
                .OrderByDescending(e => e.EnrolDate)
                .ToListAsync();

            return View(enrolments);
        }
    }
}