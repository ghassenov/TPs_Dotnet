using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesCrudApp.Data;
using MoviesCrudApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesCrudApp.Controllers
{
    public class MembershipTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembershipTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MembershipTypes
        public async Task<IActionResult> Index()
        {
            var membershipTypes = await _context.MembershipTypes.ToListAsync();
            return View(membershipTypes);
        }

        // GET: MembershipTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var membershipType = await _context.MembershipTypes
                .Include(m => m.Customers)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (membershipType == null)
                return NotFound();

            return View(membershipType);
        }

        // GET: MembershipTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MembershipTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MembershipType membershipType)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return View(membershipType);
            }

            try
            {
                _context.MembershipTypes.Add(membershipType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Errors = new List<string> { $"Erreur lors de la création: {ex.Message}" };
                return View(membershipType);
            }
        }

        // GET: MembershipTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var membershipType = await _context.MembershipTypes.FindAsync(id);
            if (membershipType == null)
                return NotFound();

            return View(membershipType);
        }

        // POST: MembershipTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MembershipType membershipType)
        {
            if (id != membershipType.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return View(membershipType);
            }

            try
            {
                _context.Update(membershipType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Errors = new List<string> { $"Erreur lors de la modification: {ex.Message}" };
                return View(membershipType);
            }
        }

        // GET: MembershipTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var membershipType = await _context.MembershipTypes.FindAsync(id);
            if (membershipType == null)
                return NotFound();

            return View(membershipType);
        }

        // POST: MembershipTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var membershipType = await _context.MembershipTypes.FindAsync(id);
                if (membershipType != null)
                {
                    _context.MembershipTypes.Remove(membershipType);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erreur lors de la suppression: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
