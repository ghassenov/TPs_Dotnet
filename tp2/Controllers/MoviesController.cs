using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesCrudApp.Data;
using MoviesCrudApp.Models;
using MoviesCrudApp.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesCrudApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly int PageSize = 5; // Nombre d'éléments par page

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movies avec pagination et tri
        public async Task<IActionResult> Index(
            string sortBy = "Title",
            string sortOrder = "asc",
            string searchString = "",
            int page = 1)
        {
            ViewData["CurrentSort"] = sortBy;
            ViewData["CurrentOrder"] = sortOrder;
            ViewData["CurrentFilter"] = searchString;

            // Requête de base avec include du Genre
            var moviesQuery = _context.Movies.Include(m => m.Genre).AsQueryable();

            // Filtrage
            if (!string.IsNullOrEmpty(searchString))
            {
                moviesQuery = moviesQuery.Where(m =>
                    m.Title.Contains(searchString) ||
                    m.Description.Contains(searchString) ||
                    m.Genre.Name.Contains(searchString));
            }

            // Tri dynamique
            moviesQuery = sortBy switch
            {
                "Title" => sortOrder == "asc" 
                    ? moviesQuery.OrderBy(m => m.Title) 
                    : moviesQuery.OrderByDescending(m => m.Title),
                "ReleaseDate" => sortOrder == "asc" 
                    ? moviesQuery.OrderBy(m => m.ReleaseDate) 
                    : moviesQuery.OrderByDescending(m => m.ReleaseDate),
                "Rating" => sortOrder == "asc" 
                    ? moviesQuery.OrderBy(m => m.Rating) 
                    : moviesQuery.OrderByDescending(m => m.Rating),
                "Duration" => sortOrder == "asc" 
                    ? moviesQuery.OrderBy(m => m.Duration) 
                    : moviesQuery.OrderByDescending(m => m.Duration),
                "Genre" => sortOrder == "asc" 
                    ? moviesQuery.OrderBy(m => m.Genre.Name) 
                    : moviesQuery.OrderByDescending(m => m.Genre.Name),
                _ => moviesQuery.OrderBy(m => m.Title)
            };

            // Pagination
            var totalItems = await moviesQuery.CountAsync();
            var movies = await moviesQuery
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var viewModel = new MovieListViewModel
            {
                Movies = movies,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = totalItems
                },
                SortBy = sortBy,
                SortOrder = sortOrder,
                CurrentFilter = searchString
            };

            return View(viewModel);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewBag.Genres = _context.Genres.OrderBy(g => g.Name).ToList();
            return View();
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,ReleaseDate,GenreId,Rating,Duration")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Genres = _context.Genres.OrderBy(g => g.Name).ToList();
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewBag.Genres = _context.Genres.OrderBy(g => g.Name).ToList();
            return View(movie);
        }

        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ReleaseDate,GenreId,Rating,Duration")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Genres = _context.Genres.OrderBy(g => g.Name).ToList();
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
