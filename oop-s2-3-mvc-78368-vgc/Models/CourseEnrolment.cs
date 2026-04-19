using System.ComponentModel.DataAnnotations;

namespace oop_s2_3_mvc_78368_vgc.Models
{
    public class CourseEnrolment
    {
        public int Id { get; set; }

        [Required]
        public int StudentProfileId { get; set; }
        public StudentProfile StudentProfile { get; set; }

        [Required]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public DateTime EnrolDate { get; set; }

        [Required]
        public string Status { get; set; }

        public ICollection<AttendanceRecord>? AttendanceRecords { get; set; }
    }
}
