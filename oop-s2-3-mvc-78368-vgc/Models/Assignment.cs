using System.ComponentModel.DataAnnotations;

namespace oop_s2_3_mvc_78368_vgc.Models
{
    public class Assignment
    {
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int MaxScore { get; set; }

        public DateTime DueDate { get; set; }

        public ICollection<AssignmentResult> Results { get; set; }
    }
}
