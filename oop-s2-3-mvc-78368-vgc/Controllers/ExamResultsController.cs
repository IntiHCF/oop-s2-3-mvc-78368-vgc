using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_3_mvc_78368_vgc.Data;
using oop_s2_3_mvc_78368_vgc.Models;

namespace oop_s2_3_mvc_78368_vgc.Controllers
{
    [Authorize(Roles = "Faculty")]
    public class ExamResultsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ExamResultsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // =========================
        // INDEX (FILTERED)
        // =========================
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);

            var faculty = _context.FacultyProfiles
                .FirstOrDefault(f => f.IdentityUserId == userId);

            if (faculty == null) return Unauthorized();

            var results = _context.ExamResults
                .Include(r => r.StudentProfile)
                .Include(r => r.Exam)
                .ThenInclude(e => e.Course)
                .Where(r => r.Exam.Course.FacultyId == faculty.Id)
                .ToList();

            return View(results);
        }

        // =========================
        // CREATE (GET)
        // =========================
        public IActionResult Create()
        {
            var userId = _userManager.GetUserId(User);

            var faculty = _context.FacultyProfiles
                .FirstOrDefault(f => f.IdentityUserId == userId);

            if (faculty == null) return Unauthorized();

            ViewBag.Exams = _context.Exams
                .Include(e => e.Course)
                .Where(e => e.Course.FacultyId == faculty.Id)
                .ToList();

            ViewBag.Students = _context.CourseEnrolments
                .Include(e => e.StudentProfile)
                .Where(e => e.Course.FacultyId == faculty.Id)
                .Select(e => e.StudentProfile)
                .Distinct()
                .ToList();

            return View();
        }

        // =========================
        // CREATE (POST)
        // =========================
        [HttpPost]
        public IActionResult Create(ExamResult result)
        {
            var userId = _userManager.GetUserId(User);

            var faculty = _context.FacultyProfiles
                .FirstOrDefault(f => f.IdentityUserId == userId);

            if (faculty == null) return Unauthorized();

            if (!ModelState.IsValid)
            {
                // 🔥 reload dropdowns
                ViewBag.Exams = _context.Exams
                    .Include(e => e.Course)
                    .Where(e => e.Course.FacultyId == faculty.Id)
                    .ToList();

                ViewBag.Students = _context.CourseEnrolments
                    .Include(e => e.StudentProfile)
                    .Where(e => e.Course.FacultyId == faculty.Id)
                    .Select(e => e.StudentProfile)
                    .Distinct()
                    .ToList();

                return View(result);
            }

            var exam = _context.Exams
                .Include(e => e.Course)
                .FirstOrDefault(e => e.Id == result.ExamId);

            if (exam == null) return NotFound();

            // 🔐 SECURITY CHECK
            if (exam.Course.FacultyId != faculty.Id)
            {
                return Unauthorized();
            }

            _context.ExamResults.Add(result);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // EDIT (GET)
        // =========================
        public IActionResult Edit(int id)
        {
            var userId = _userManager.GetUserId(User);

            var faculty = _context.FacultyProfiles
                .FirstOrDefault(f => f.IdentityUserId == userId);

            if (faculty == null) return Unauthorized();

            var result = _context.ExamResults
                .Include(r => r.Exam)
                .ThenInclude(e => e.Course)
                .FirstOrDefault(r => r.Id == id);

            if (result == null) return NotFound();

            // 🔐 SECURITY
            if (result.Exam.Course.FacultyId != faculty.Id)
            {
                return Unauthorized();
            }

            return View(result);
        }

        // =========================
        // EDIT (POST)
        // =========================
        [HttpPost]
        public IActionResult Edit(ExamResult result)
        {
            var userId = _userManager.GetUserId(User);

            var faculty = _context.FacultyProfiles
                .FirstOrDefault(f => f.IdentityUserId == userId);

            if (faculty == null) return Unauthorized();

            if (!ModelState.IsValid)
            {
                return View(result);
            }

            var exam = _context.Exams
                .Include(e => e.Course)
                .FirstOrDefault(e => e.Id == result.ExamId);

            if (exam == null) return NotFound();

            // 🔐 SECURITY
            if (exam.Course.FacultyId != faculty.Id)
            {
                return Unauthorized();
            }

            _context.ExamResults.Update(result);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DELETE (GET)
        // =========================
        public IActionResult Delete(int id)
        {
            var userId = _userManager.GetUserId(User);

            var faculty = _context.FacultyProfiles
                .FirstOrDefault(f => f.IdentityUserId == userId);

            if (faculty == null) return Unauthorized();

            var result = _context.ExamResults
                .Include(r => r.Exam)
                .ThenInclude(e => e.Course)
                .Include(r => r.StudentProfile)
                .FirstOrDefault(r => r.Id == id);

            if (result == null) return NotFound();

            // 🔐 SECURITY
            if (result.Exam.Course.FacultyId != faculty.Id)
            {
                return Unauthorized();
            }

            return View(result);
        }

        // =========================
        // DELETE (POST)
        // =========================
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);

            var faculty = _context.FacultyProfiles
                .FirstOrDefault(f => f.IdentityUserId == userId);

            if (faculty == null) return Unauthorized();

            var result = _context.ExamResults
                .Include(r => r.Exam)
                .ThenInclude(e => e.Course)
                .FirstOrDefault(r => r.Id == id);

            if (result == null) return NotFound();

            // 🔐 SECURITY
            if (result.Exam.Course.FacultyId != faculty.Id)
            {
                return Unauthorized();
            }

            _context.ExamResults.Remove(result);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
