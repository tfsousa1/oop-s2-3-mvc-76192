using System.ComponentModel.DataAnnotations;

namespace AcmeGlobalCollege.Web.Models
{
    public class FacultyCourseAssignment
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Faculty")]
        public int FacultyProfileId { get; set; }

        public FacultyProfile? FacultyProfile { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        public Course? Course { get; set; }

        [Display(Name = "Tutor")]
        public bool IsTutor { get; set; }
    }
}