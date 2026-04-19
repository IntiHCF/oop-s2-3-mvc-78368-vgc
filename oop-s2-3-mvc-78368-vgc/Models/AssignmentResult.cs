using System.ComponentModel.DataAnnotations;

namespace oop_s2_3_mvc_78368_vgc.Models
{
    public class AssignmentResult
    {
        public int Id { get; set; }

        [Required]
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }

        [Required]
        public int StudentProfileId { get; set; }
        public StudentProfile StudentProfile { get; set; }

        [Required]
        public int Score { get; set; }

        public string? Feedback { get; set; }
    }
}
