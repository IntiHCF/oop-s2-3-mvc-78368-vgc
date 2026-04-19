using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_3_mvc_78368_vgc.Data;

namespace oop_s2_3_mvc_78368_vgc.Controllers
{
    [Authorize(Roles = "Student")]
    public class MyAttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MyAttendanceController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // READ ONLY
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);

            var student = _context.StudentProfiles
                .FirstOrDefault(s => s.IdentityUserId == userId);

            if (student == null) return Unauthorized();

            var attendance = _context.AttendanceRecords
                .Include(a => a.CourseEnrolment)
                .ThenInclude(e => e.Course)
                .Where(a => a.CourseEnrolment.StudentProfileId == student.Id)
                .ToList();

            return View(attendance);
        }
    }
}
