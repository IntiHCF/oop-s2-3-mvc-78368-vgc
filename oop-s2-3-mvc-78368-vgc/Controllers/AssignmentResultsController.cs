using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_3_mvc_78368_vgc.Data;
using oop_s2_3_mvc_78368_vgc.Models;

namespace oop_s2_3_mvc_78368_vgc.Controllers
{
    [Authorize(Roles = "Faculty")]
    public class AssignmentResultsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AssignmentResultsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // -------------------------
        // INDEX (FILTERED)
        // -------------------------
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);

            var faculty = _context.FacultyProfiles
                .First(f => f.IdentityUserId == userId);

            var results = _context.AssignmentResults
                .Include(r => r.StudentProfile)
                .Include(r => r.Assignment)
                .Where(r => r.Assignment.Course.FacultyId == faculty.Id)
                .ToList();

            return View(results);
        }

        // -------------------------
        // CREATE (FILTERED DATA)
        // -------------------------
        public IActionResult Create()
        {
            var userId = _userManager.GetUserId(User);

            var faculty = _context.FacultyProfiles
                .First(f => f.IdentityUserId == userId);

            var assignments = _context.Assignments
                .Where(a => a.Course.FacultyId == faculty.Id)
                .ToList();

            var students = _context.CourseEnrolments
                .Include(e => e.StudentProfile)
                .Where(e => e.Course.FacultyId == faculty.Id)
                .Select(e => e.StudentProfile)
                .Distinct()
                .ToList();

            ViewBag.Students = students;
            ViewBag.Assignments = assignments;

            return View();
        }

        // -------------------------
        // POST (SECURE CHECK)
        // -------------------------
        [HttpPost]
        public IActionResult Create(AssignmentResult result)
        {
            var userId = _userManager.GetUserId(User);

            var faculty = _context.FacultyProfiles
                .FirstOrDefault(f => f.IdentityUserId == userId);

            if (faculty == null)
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                // 🔥 RELOAD DROPDOWNS (THIS WAS MISSING)
                ViewBag.Assignments = _context.Assignments
                    .Where(a => a.Course.FacultyId == faculty.Id)
                    .ToList();

                ViewBag.Students = _context.CourseEnrolments
                    .Include(e => e.StudentProfile)
                    .Where(e => e.Course.FacultyId == faculty.Id)
                    .Select(e => e.StudentProfile)
                    .Distinct()
                    .ToList();

                return View(result);
            }

            var assignment = _context.Assignments
                .Include(a => a.Course)
                .FirstOrDefault(a => a.Id == result.AssignmentId);

            if (assignment == null)
                return NotFound();

            if (assignment.Course.FacultyId != faculty.Id)
                return Unauthorized();

            _context.AssignmentResults.Add(result);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
