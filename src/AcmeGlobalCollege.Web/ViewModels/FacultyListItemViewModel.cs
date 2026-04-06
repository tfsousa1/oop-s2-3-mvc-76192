namespace AcmeGlobalCollege.Web.ViewModels
{
    public class FacultyListItemViewModel
    {
        public int FacultyProfileId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public List<string> AssignedCourses { get; set; } = new();
    }
}