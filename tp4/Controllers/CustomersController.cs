using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesCrudApp.Data;
using MoviesCrudApp.Models;
using MoviesCrudApp.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesCrudApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly int PageSize = 10;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string searchString = "", int page = 1)
        {
            ViewData["CurrentFilter"] = searchString;

            var customersQuery = _context.Customers
                .Include(c => c.MembershipType)
                .AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(searchString))
            {
                customersQuery = customersQuery.Where(c =>
                    c.FirstName.Contains(searchString) ||
                    c.LastName.Contains(searchString) ||
                    c.Email.Contains(searchString));
            }

            // Pagination
            var totalCount = await customersQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            var customers = await customersQuery
                .OrderBy(c => c.LastName)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var model = new CustomerListViewModel
            {
                Customers = customers,
                CurrentPage = page,
                TotalPages = totalPages,
                SearchString = searchString
            };

            return View(model);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewBag.MembershipTypes = _context.MembershipTypes.ToList();
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            ViewBag.MembershipTypes = _context.MembershipTypes.ToList();

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return View(customer);
            }

            try
            {
                customer.RegistrationDate = DateTime.Now;
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Errors = new List<string> { $"Erreur lors de la création du client: {ex.Message}" };
                return View(customer);
            }
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();

            ViewBag.MembershipTypes = _context.MembershipTypes.ToList();
            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.Id)
                return NotFound();

            ViewBag.MembershipTypes = _context.MembershipTypes.ToList();

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return View(customer);
            }

            try
            {
                _context.Update(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Errors = new List<string> { $"Erreur lors de la modification: {ex.Message}" };
                return View(customer);
            }
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var customer = await _context.Customers
                .Include(c => c.MembershipType)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer != null)
                {
                    _context.Customers.Remove(customer);
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
