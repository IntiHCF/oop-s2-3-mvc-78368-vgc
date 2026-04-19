using System.ComponentModel.DataAnnotations;

namespace oop_s2_3_mvc_78368_vgc.Models
{
    public class ExamResult
    {
        public int Id { get; set; }

        [Required]
        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        [Required]
        public int StudentProfileId { get; set; }
        public StudentProfile StudentProfile { get; set; }

        [Required]
        public int Score { get; set; }

        public string? Grade { get; set; }
    }
}
