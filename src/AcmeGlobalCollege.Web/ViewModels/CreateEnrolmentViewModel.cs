using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AcmeGlobalCollege.Web.ViewModels
{
    public class CreateEnrolmentViewModel
    {
        [Required]
        [Display(Name = "Student")]
        public int StudentProfileId { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Enrolment Date")]
        public DateTime EnrolDate { get; set; } = DateTime.Today;

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Active";

        public List<SelectListItem> Students { get; set; } = new();
        public List<SelectListItem> Courses { get; set; } = new();
    }
}