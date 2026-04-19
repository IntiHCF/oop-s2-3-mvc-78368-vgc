using System.ComponentModel.DataAnnotations;    

namespace oop_s2_3_mvc_78368_vgc.Models
{
    public class AttendanceRecord
    {
        public int Id { get; set; }

        [Required]
        public int CourseEnrolmentId { get; set; }
        public CourseEnrolment CourseEnrolment { get; set; }

        public DateTime Date { get; set; }

        public int WeekNumber { get; set; }

        public bool Present { get; set; }
    }
}
