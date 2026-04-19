using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace oop_s2_3_mvc_78368_vgc.Models
{
    public class StudentProfile
    {
        public int Id { get; set; }

        [Required]
        public string IdentityUserId { get; set; }
        public IdentityUser User { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? Phone { get; set; }
        public string? Address { get; set; }

        [Required]
        public string StudentNumber { get; set; }

        public ICollection<CourseEnrolment>? Enrolments { get; set; }
        public ICollection<AssignmentResult>? AssignmentResults { get; set; }
        public ICollection<ExamResult>? ExamResults { get; set; }
    }
}
