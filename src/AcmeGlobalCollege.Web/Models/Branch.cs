using System.ComponentModel.DataAnnotations;

namespace AcmeGlobalCollege.Web.Models
{
    public class Branch
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Branch Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}