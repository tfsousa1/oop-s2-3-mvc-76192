using System.ComponentModel.DataAnnotations;

namespace AcmeGlobalCollege.Web.Models
{
    public class ExamResult
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Exam")]
        public int ExamId { get; set; }

        public Exam? Exam { get; set; }

        [Required]
        [Display(Name = "Student")]
        public int StudentProfileId { get; set; }

        public StudentProfile? StudentProfile { get; set; }

        [Range(0, 1000)]
        public int Score { get; set; }

        [Required]
        [StringLength(10)]
        public string Grade { get; set; } = string.Empty;
    }
}