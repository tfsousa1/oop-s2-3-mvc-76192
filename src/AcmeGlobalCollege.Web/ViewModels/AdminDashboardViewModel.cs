using AcmeGlobalCollege.Web.Models;

namespace AcmeGlobalCollege.Web.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalBranches { get; set; }
        public int TotalCourses { get; set; }
        public int TotalFaculty { get; set; }
        public int TotalStudents { get; set; }
        public int TotalEnrolments { get; set; }

        public List<Branch> Branches { get; set; } = new();
        public List<Course> Courses { get; set; } = new();
    }
}