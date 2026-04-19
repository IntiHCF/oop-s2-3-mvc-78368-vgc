using System.ComponentModel.DataAnnotations;

namespace oop_s2_3_mvc_78368_vgc.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required]
        public int FacultyId { get; set; }
        public FacultyProfile Faculty { get; set; }

        public ICollection<CourseEnrolment>? Enrolments { get; set; }
        public ICollection<Assignment>? Assignments { get; set; }
        public ICollection<Exam>? Exams { get; set; }
    }
}
