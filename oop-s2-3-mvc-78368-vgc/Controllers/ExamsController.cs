using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_3_mvc_78368_vgc.Data;
using oop_s2_3_mvc_78368_vgc.Models;

namespace oop_s2_3_mvc_78368_vgc.Controllers
{
    [Authorize(Roles = "Admin,Faculty")]
    public class ExamsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ExamsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // =========================
        // INDEX
        // =========================
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View(_context.Exams.Include(e => e.Course).ToList());
            }

            var userId = _userManager.GetUserId(User);

            var faculty = _context.FacultyProfiles
                .FirstOrDefault(f => f.IdentityUserId == userId);

            if (faculty == null) return Unauthorized();

            var exams = _context.Exams
                .Include(e => e.Course)
                .Where(e => e.Course.FacultyId == faculty.Id)
                .ToList();

            return View(exams);
        }

        // =========================
        // CREATE (GET)
        // =========================
        public IActionResult Create()
        {
            if (User.IsInRole("Faculty"))
            {
                var userId = _userManager.GetUserId(User);

                var faculty = _context.FacultyProfiles
                    .FirstOrDefault(f => f.IdentityUserId == userId);

                ViewBag.Courses = _context.Courses
                    .Where(c => c.FacultyId == faculty.Id)
                    .ToList();
            }
            else
            {
                ViewBag.Courses = _context.Courses.ToList();
            }

            return View();
        }

        // =========================
        // CREATE (POST)
        // =========================
        [HttpPost]
        public IActionResult Create(Exam exam)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Courses = _context.Courses.ToList();
                return View(exam);
            }

            if (User.IsInRole("Faculty"))
            {
                var userId = _userManager.GetUserId(User);

                var faculty = _context.FacultyProfiles
                    .FirstOrDefault(f => f.IdentityUserId == userId);

                var course = _context.Courses
                    .FirstOrDefault(c => c.Id == exam.CourseId);

                if (faculty == null || course == null || course.FacultyId != faculty.Id)
                {
                    return Unauthorized();
                }
            }

            _context.Exams.Add(exam);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // EDIT (GET)
        // =========================
        public IActionResult Edit(int id)
        {
            var exam = _context.Exams
                .Include(e => e.Course)
                .FirstOrDefault(e => e.Id == id);

            if (exam == null) return NotFound();

            if (User.IsInRole("Faculty"))
            {
                var userId = _userManager.GetUserId(User);

                var faculty = _context.FacultyProfiles
                    .FirstOrDefault(f => f.IdentityUserId == userId);

                if (faculty == null || exam.Course.FacultyId != faculty.Id)
                {
                    return Unauthorized();
                }

                ViewBag.Courses = _context.Courses
                    .Where(c => c.FacultyId == faculty.Id)
                    .ToList();
            }
            else
            {
                ViewBag.Courses = _context.Courses.ToList();
            }

            return View(exam);
        }

        // =========================
        // EDIT (POST)
        // =========================
        [HttpPost]
        public IActionResult Edit(Exam exam)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Courses = _context.Courses.ToList();
                return View(exam);
            }

            if (User.IsInRole("Faculty"))
            {
                var userId = _userManager.GetUserId(User);

                var faculty = _context.FacultyProfiles
                    .FirstOrDefault(f => f.IdentityUserId == userId);

                var course = _context.Courses
                    .FirstOrDefault(c => c.Id == exam.CourseId);

                if (faculty == null || course == null || course.FacultyId != faculty.Id)
                {
                    return Unauthorized();
                }
            }

            _context.Exams.Update(exam);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DELETE (GET)
        // =========================
        public IActionResult Delete(int id)
        {
            var exam = _context.Exams
                .Include(e => e.Course)
                .FirstOrDefault(e => e.Id == id);

            if (exam == null) return NotFound();

            if (User.IsInRole("Faculty"))
            {
                var userId = _userManager.GetUserId(User);

                var faculty = _context.FacultyProfiles
                    .FirstOrDefault(f => f.IdentityUserId == userId);

                if (faculty == null || exam.Course.FacultyId != faculty.Id)
                {
                    return Unauthorized();
                }
            }

            return View(exam);
        }

        // =========================
        // DELETE (POST)
        // =========================
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var exam = _context.Exams
                .Include(e => e.Course)
                .FirstOrDefault(e => e.Id == id);

            if (exam == null) return NotFound();

            if (User.IsInRole("Faculty"))
            {
                var userId = _userManager.GetUserId(User);

                var faculty = _context.FacultyProfiles
                    .FirstOrDefault(f => f.IdentityUserId == userId);

                if (faculty == null || exam.Course.FacultyId != faculty.Id)
                {
                    return Unauthorized();
                }
            }

            _context.Exams.Remove(exam);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
