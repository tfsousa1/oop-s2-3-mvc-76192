using System.ComponentModel.DataAnnotations;

namespace AcmeGlobalCollege.Web.Models
{
    public class AttendanceRecord
    {
        public int Id { get; set; }

        [Required]
        public int CourseEnrolmentId { get; set; }

        public CourseEnrolment? CourseEnrolment { get; set; }

        [Required]
        [Display(Name = "Week Number")]
        public int WeekNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Session Date")]
        public DateTime SessionDate { get; set; }

        [Display(Name = "Present")]
        public bool Present { get; set; }
    }
}