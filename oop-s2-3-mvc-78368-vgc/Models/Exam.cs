using System.ComponentModel.DataAnnotations;

namespace oop_s2_3_mvc_78368_vgc.Models
{
    public class Exam
    {
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public int MaxScore { get; set; }

        public bool ResultsReleased { get; set; }

        public ICollection<ExamResult>? Results { get; set; }
    }
}
