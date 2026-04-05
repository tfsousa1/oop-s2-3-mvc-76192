using System.ComponentModel.DataAnnotations;

namespace AcmeGlobalCollege.Web.Models
{
    public class Exam
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        public Course? Course { get; set; }

        [Required]
        [StringLength(120)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Range(1, 1000)]
        [Display(Name = "Maximum Score")]
        public int MaxScore { get; set; }

        [Display(Name = "Results Released")]
        public bool ResultsReleased { get; set; }

        public ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
    }
}