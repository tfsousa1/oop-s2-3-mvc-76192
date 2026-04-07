using AcmeGlobalCollege.Web.Models;

namespace AcmeGlobalCollege.Web.ViewModels
{
    public class CourseDetailsViewModel
    {
        // Main course record displayed on the page.
        public Course Course { get; set; } = default!;

        // Modules linked to the selected course.
        public List<Module> Modules { get; set; } = new();

        // Enrolments linked to the selected course.
        public List<CourseEnrolment> Enrolments { get; set; } = new();
    }
}