using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_3_mvc_78368_vgc.Data;
using oop_s2_3_mvc_78368_vgc.Models;

namespace oop_s2_3_mvc_78368_vgc.Controllers
{
    [Authorize(Roles = "Admin,Faculty")]
    public class AssignmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AssignmentsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
                return View(_context.Assignments.Include(a => a.Course).ToList());

            var userId = _userManager.GetUserId(User);
            var faculty = _context.FacultyProfiles.First(f => f.IdentityUserId == userId);

            var assignments = _context.Assignments
                .Include(a => a.Course)
                .Where(a => a.Course.FacultyId == faculty.Id)
                .ToList();

            return View(assignments);
        }

        public IActionResult Create()
        {
            ViewBag.Courses = _context.Courses.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Assignment assignment)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Courses = _context.Courses.ToList();
                return View(assignment);
            }

            _context.Assignments.Add(assignment);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var assignment = _context.Assignments.FirstOrDefault(a => a.Id == id);
            if (assignment == null) return NotFound();

            ViewBag.Courses = _context.Courses.ToList();
            return View(assignment);
        }

        [HttpPost]
        public IActionResult Edit(Assignment assignment)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Courses = _context.Courses.ToList();
                return View(assignment);
            }

            _context.Assignments.Update(assignment);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var assignment = _context.Assignments
                .Include(a => a.Course)
                .FirstOrDefault(a => a.Id == id);

            if (assignment == null) return NotFound();

            return View(assignment);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var assignment = _context.Assignments.FirstOrDefault(a => a.Id == id);
            if (assignment == null) return NotFound();

            _context.Assignments.Remove(assignment);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
