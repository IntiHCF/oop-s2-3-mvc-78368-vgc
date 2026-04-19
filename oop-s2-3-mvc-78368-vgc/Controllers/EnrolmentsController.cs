using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_3_mvc_78368_vgc.Data;
using oop_s2_3_mvc_78368_vgc.Models;

namespace oop_s2_3_mvc_78368_vgc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EnrolmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrolmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.CourseEnrolments
                .Include(e => e.StudentProfile)
                .Include(e => e.Course)
                .ToList());
        }

        public IActionResult Create()
        {
            ViewBag.Students = _context.StudentProfiles.ToList();
            ViewBag.Courses = _context.Courses.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CourseEnrolment enrolment)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Students = _context.StudentProfiles.ToList();
                ViewBag.Courses = _context.Courses.ToList();
                return View(enrolment);
            }

            _context.CourseEnrolments.Add(enrolment);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var enrolment = _context.CourseEnrolments.FirstOrDefault(e => e.Id == id);
            if (enrolment == null) return NotFound();

            ViewBag.Students = _context.StudentProfiles.ToList();
            ViewBag.Courses = _context.Courses.ToList();

            return View(enrolment);
        }

        [HttpPost]
        public IActionResult Edit(CourseEnrolment enrolment)
        {
            if (!ModelState.IsValid) return View(enrolment);

            _context.CourseEnrolments.Update(enrolment);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var enrolment = _context.CourseEnrolments
                .Include(e => e.StudentProfile)
                .Include(e => e.Course)
                .FirstOrDefault(e => e.Id == id);

            if (enrolment == null) return NotFound();

            return View(enrolment);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var enrolment = _context.CourseEnrolments.FirstOrDefault(e => e.Id == id);
            if (enrolment == null) return NotFound();

            _context.CourseEnrolments.Remove(enrolment);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
