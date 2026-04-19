using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_3_mvc_78368_vgc.Data;
using oop_s2_3_mvc_78368_vgc.Models;

namespace oop_s2_3_mvc_78368_vgc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // -------------------------
        // INDEX
        // -------------------------
        public IActionResult Index()
        {
            var courses = _context.Courses
                .Include(c => c.Branch)
                .Include(c => c.Faculty)
                .ToList();

            return View(courses);
        }

        // -------------------------
        // CREATE (GET)
        // -------------------------
        public IActionResult Create()
        {
            ViewBag.Branches = _context.Branches.ToList();
            ViewBag.Faculties = _context.FacultyProfiles.ToList();

            return View();
        }

        // -------------------------
        // CREATE (POST)
        // -------------------------
        [HttpPost]
        public IActionResult Create(Course course)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Branches = _context.Branches.ToList();
                ViewBag.Faculties = _context.FacultyProfiles.ToList();
                return View(course);
            }

            _context.Courses.Add(course);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // -------------------------
        // EDIT (GET)
        // -------------------------
        public IActionResult Edit(int id)
        {
            var course = _context.Courses.Find(id);

            if (course == null)
                return NotFound();

            ViewBag.Branches = _context.Branches.ToList();
            ViewBag.Faculties = _context.FacultyProfiles.ToList();

            return View(course);
        }

        // -------------------------
        // EDIT (POST)
        // -------------------------
        [HttpPost]
        public IActionResult Edit(Course course)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Branches = _context.Branches.ToList();
                ViewBag.Faculties = _context.FacultyProfiles.ToList();
                return View(course);
            }

            _context.Courses.Update(course);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // -------------------------
        // DELETE (GET)
        // -------------------------
        public IActionResult Delete(int id)
        {
            var course = _context.Courses
                .Include(c => c.Branch)
                .Include(c => c.Faculty)
                .FirstOrDefault(c => c.Id == id);

            if (course == null)
                return NotFound();

            return View(course);
        }

        // -------------------------
        // DELETE (POST)
        // -------------------------
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = _context.Courses.Find(id);

            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
