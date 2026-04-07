using AcmeGlobalCollege.Web.Data;
using AcmeGlobalCollege.Web.Models;
using AcmeGlobalCollege.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // Lists all enrolments with student and course information.
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

        // Shows one enrolment in a dedicated details page.
        public async Task<IActionResult> Details(int id)
        {
            var enrolment = await _context.CourseEnrolments
                .Include(e => e.StudentProfile)
                .Include(e => e.Course)
                    .ThenInclude(c => c!.Branch)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enrolment == null)
            {
                return NotFound();
            }

            return View(enrolment);
        }

        // Loads dropdowns for the create page.
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateEnrolmentViewModel();
            await PopulateCreateListsAsync(viewModel);

            return View(viewModel);
        }

        // Saves a new enrolment and prevents duplicate active enrolments.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEnrolmentViewModel viewModel)
        {
            if (await _context.CourseEnrolments.AnyAsync(e =>
                e.StudentProfileId == viewModel.StudentProfileId &&
                e.CourseId == viewModel.CourseId &&
                e.Status == "Active"))
            {
                ModelState.AddModelError(string.Empty, "This student already has an active enrolment for the selected course.");
            }

            if (!ModelState.IsValid)
            {
                await PopulateCreateListsAsync(viewModel);
                return View(viewModel);
            }

            var enrolment = new CourseEnrolment
            {
                StudentProfileId = viewModel.StudentProfileId,
                CourseId = viewModel.CourseId,
                EnrolDate = viewModel.EnrolDate,
                Status = viewModel.Status
            };

            _context.CourseEnrolments.Add(enrolment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = enrolment.Id });
        }

        // Rebuilds dropdown lists when the form is loaded or validation fails.
        private async Task PopulateCreateListsAsync(CreateEnrolmentViewModel viewModel)
        {
            viewModel.Students = await _context.StudentProfiles
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = $"{s.FullName} ({s.StudentNumber})"
                })
                .ToListAsync();

            viewModel.Courses = await _context.Courses
                .Include(c => c.Branch)
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Name} - {c.Branch!.Name}"
                })
                .ToListAsync();
        }
    }
}
