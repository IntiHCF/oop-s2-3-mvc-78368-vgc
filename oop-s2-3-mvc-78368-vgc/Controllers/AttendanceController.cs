using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_3_mvc_78368_vgc.Data;
using oop_s2_3_mvc_78368_vgc.Models;

namespace oop_s2_3_mvc_78368_vgc.Controllers
{
    [Authorize(Roles = "Faculty")]
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AttendanceController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // =========================
        // INDEX
        // =========================
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);

            var faculty = _context.FacultyProfiles
                .FirstOrDefault(f => f.IdentityUserId == userId);

            if (faculty == null) return Unauthorized();

            var attendance = _context.AttendanceRecords
                .Include(a => a.CourseEnrolment)
                    .ThenInclude(e => e.StudentProfile)
                .Include(a => a.CourseEnrolment)
                    .ThenInclude(e => e.Course)
                .Where(a => a.CourseEnrolment.Course.FacultyId == faculty.Id)
                .ToList();

            return View(attendance);
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

            ViewBag.Enrolments = _context.CourseEnrolments
                .Include(e => e.StudentProfile)
                .Include(e => e.Course)
                .Where(e => e.Course.FacultyId == faculty.Id)
                .ToList();

            return View();
        }

        // =========================
        // CREATE (POST)
        // =========================
        [HttpPost]
        public IActionResult Create(AttendanceRecord record)
        {
            var userId = _userManager.GetUserId(User);

            var faculty = _context.FacultyProfiles
                .FirstOrDefault(f => f.IdentityUserId == userId);

            if (faculty == null) return Unauthorized();

            if (!ModelState.IsValid)
            {
                ViewBag.Enrolments = _context.CourseEnrolments
                    .Include(e => e.StudentProfile)
                    .Include(e => e.Course)
                    .Where(e => e.Course.FacultyId == faculty.Id)
                    .ToList();

                return View(record);
            }

            // 🔐 SECURITY CHECK
            var enrolment = _context.CourseEnrolments
                .Include(e => e.Course)
                .FirstOrDefault(e => e.Id == record.CourseEnrolmentId);

            if (enrolment == null) return NotFound();

            if (enrolment.Course.FacultyId != faculty.Id)
            {
                return Unauthorized();
            }

            _context.AttendanceRecords.Add(record);
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

            var record = _context.AttendanceRecords
                .Include(a => a.CourseEnrolment)
                    .ThenInclude(e => e.Course)
                .Include(a => a.CourseEnrolment)
                    .ThenInclude(e => e.StudentProfile)
                .FirstOrDefault(a => a.Id == id);

            if (record == null) return NotFound();

            if (record.CourseEnrolment.Course.FacultyId != faculty.Id)
            {
                return Unauthorized();
            }

            return View(record);
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

            var record = _context.AttendanceRecords
                .Include(a => a.CourseEnrolment)
                    .ThenInclude(e => e.Course)
                .FirstOrDefault(a => a.Id == id);

            if (record == null) return NotFound();

            if (record.CourseEnrolment.Course.FacultyId != faculty.Id)
            {
                return Unauthorized();
            }

            _context.AttendanceRecords.Remove(record);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
