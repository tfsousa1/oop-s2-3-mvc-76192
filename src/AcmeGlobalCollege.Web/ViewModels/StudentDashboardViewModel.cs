using AcmeGlobalCollege.Web.Models;

namespace AcmeGlobalCollege.Web.ViewModels
{
    public class StudentDashboardViewModel
    {
        // Current logged-in student's profile.
        public StudentProfile Student { get; set; } = default!;

        // Enrolments linked to the current student.
        public List<CourseEnrolment> Enrolments { get; set; } = new();
    }
}