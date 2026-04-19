using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using oop_s2_3_mvc_78368_vgc.Data;
using oop_s2_3_mvc_78368_vgc.Models;

namespace oop_s2_3_mvc_78368_vgc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BranchesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BranchesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Branches.ToList());
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Branch branch)
        {
            if (!ModelState.IsValid) return View(branch);

            _context.Branches.Add(branch);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var branch = _context.Branches.FirstOrDefault(b => b.Id == id);
            if (branch == null) return NotFound();

            return View(branch);
        }

        [HttpPost]
        public IActionResult Edit(Branch branch)
        {
            if (!ModelState.IsValid) return View(branch);

            _context.Branches.Update(branch);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var branch = _context.Branches.FirstOrDefault(b => b.Id == id);
            if (branch == null) return NotFound();

            return View(branch);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var branch = _context.Branches.FirstOrDefault(b => b.Id == id);
            if (branch == null) return NotFound();

            _context.Branches.Remove(branch);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
