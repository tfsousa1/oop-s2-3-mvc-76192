using AcmeGlobalCollege.Web.Models;

namespace AcmeGlobalCollege.Web.ViewModels
{
    public class FacultyDashboardViewModel
    {
        // Display name shown at the top of the faculty dashboard.
        public string FacultyName { get; set; } = string.Empty;

        // Courses assigned to the logged-in faculty member.
        public List<FacultyCourseAssignment> CourseAssignments { get; set; } = new();

        // Enrolments linked to those assigned courses.
        public List<CourseEnrolment> Enrolments { get; set; } = new();
    }
}