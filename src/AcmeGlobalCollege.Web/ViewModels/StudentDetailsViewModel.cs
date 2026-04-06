using AcmeGlobalCollege.Web.Models;

namespace AcmeGlobalCollege.Web.ViewModels
{
    public class StudentDetailsViewModel
    {
        public StudentProfile Student { get; set; } = null!;
        public List<CourseEnrolment> Enrolments { get; set; } = new();
    }
}