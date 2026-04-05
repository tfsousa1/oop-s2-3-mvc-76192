using System.ComponentModel.DataAnnotations;

namespace AcmeGlobalCollege.Web.Models
{
    public class AssignmentResult
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Assignment")]
        public int AssignmentId { get; set; }

        public Assignment? Assignment { get; set; }

        [Required]
        [Display(Name = "Student")]
        public int StudentProfileId { get; set; }

        public StudentProfile? StudentProfile { get; set; }

        [Range(0, 1000)]
        public int Score { get; set; }

        [StringLength(500)]
        public string? Feedback { get; set; }
    }
}