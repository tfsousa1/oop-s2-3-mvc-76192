using AcmeGlobalCollege.Web.Models;

namespace AcmeGlobalCollege.Web.ViewModels
{
    public class CourseDetailsViewModel
    {
        public Course Course { get; set; } = null!;
        public List<Module> Modules { get; set; } = new();
        public List<FacultyCourseAssignment> FacultyAssignments { get; set; } = new();
        public List<CourseEnrolment> Enrolments { get; set; } = new();
    }
}