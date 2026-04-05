using System.ComponentModel.DataAnnotations;

namespace AcmeGlobalCollege.Web.Models
{
    public class Module
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        [Display(Name = "Module Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        public Course? Course { get; set; }
    }
}