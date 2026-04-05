using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AcmeGlobalCollege.Web.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        [Display(Name = "Course Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        public Branch? Branch { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public ICollection<Module> Modules { get; set; } = new List<Module>();
    }
}