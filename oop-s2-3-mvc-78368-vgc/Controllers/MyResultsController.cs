using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_3_mvc_78368_vgc.Data;

namespace oop_s2_3_mvc_78368_vgc.Controllers
{
    [Authorize(Roles = "Student")]
    public class MyResultsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MyResultsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);

            var student = _context.StudentProfiles
                .FirstOrDefault(s => s.IdentityUserId == userId);

            if (student == null) return Unauthorized();

            var examResults = _context.ExamResults
                .Include(r => r.Exam)
                .ThenInclude(e => e.Course)
                .Where(r => r.StudentProfileId == student.Id)
                .ToList();

            var assignmentResults = _context.AssignmentResults
                .Include(r => r.Assignment)
                .ThenInclude(a => a.Course)
                .Where(r => r.StudentProfileId == student.Id)
                .ToList();

            ViewBag.ExamResults = examResults;
            ViewBag.AssignmentResults = assignmentResults;

            return View();
        }
    }
}
