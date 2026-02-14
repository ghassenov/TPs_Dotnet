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
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly int PageSize = 5;

        public MoviesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Movies with pagination and search
        public async Task<IActionResult> Index(
            string sortBy = "Title",
            string sortOrder = "asc",
            string searchString = "",
            int page = 1)
        {
            ViewData["CurrentSort"] = sortBy;
            ViewData["CurrentOrder"] = sortOrder;
            ViewData["CurrentFilter"] = searchString;

            var moviesQuery = _context.Movies.Include(m => m.Genre).AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(searchString))
            {
                moviesQuery = moviesQuery.Where(m =>
                    m.Title.Contains(searchString) ||
                    m.Description.Contains(searchString) ||
                    m.Genre.Name.Contains(searchString));
            }

            // Dynamic sorting
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
                _ => moviesQuery.OrderBy(m => m.Title)
            };

            // Pagination
            var totalCount = await moviesQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            var movies = await moviesQuery
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var model = new MovieListViewModel
            {
                Movies = movies,
                CurrentPage = page,
                TotalPages = totalPages,
                SortBy = sortBy,
                SortOrder = sortOrder,
                SearchString = searchString
            };

            return View(model);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var movie = await _context.Movies
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound();

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewBag.Genres = _context.Genres.ToList();
            return View(new MovieVM());
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieVM viewModel)
        {
            ViewBag.Genres = _context.Genres.ToList();

            // Remove Genre validation since it's a navigation property
            ModelState.Remove("Movie.Genre");

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return View(viewModel);
            }

            try
            {
                // Handle file upload
                if (viewModel.Photo != null && viewModel.Photo.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.Photo.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.Photo.CopyToAsync(fileStream);
                    }

                    viewModel.Movie.ImageFile = fileName;
                }

                // Set the date of addition
                viewModel.Movie.DateAjoutMovie = DateTime.Now;

                _context.Movies.Add(viewModel.Movie);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Errors = new List<string> { 
                    $"Erreur lors de la création du film: {ex.Message}",
                    ex.InnerException?.Message
                };
                return View(viewModel);
            }
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound();

            ViewBag.Genres = _context.Genres.ToList();
            return View(movie);
        }

        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie, IFormFile? Photo, bool RemoveImage = false)
        {
            if (id != movie.Id)
                return NotFound();

            ViewBag.Genres = _context.Genres.ToList();

            // Remove Genre validation since it's a navigation property
            ModelState.Remove("Genre");

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return View(movie);
            }

            try
            {
                // Get the existing movie to preserve ImageFile and DateAjoutMovie
                var existingMovie = await _context.Movies
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == id);
                    
                if (existingMovie == null)
                    return NotFound();

                // Handle image removal
                if (RemoveImage && !string.IsNullOrEmpty(existingMovie.ImageFile))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", existingMovie.ImageFile);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                    movie.ImageFile = null;
                }
                // Handle new photo upload
                else if (Photo != null && Photo.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Photo.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Photo.CopyToAsync(fileStream);
                    }

                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(existingMovie.ImageFile))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", existingMovie.ImageFile);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    movie.ImageFile = fileName;
                }
                else
                {
                    // Keep existing image
                    movie.ImageFile = existingMovie.ImageFile;
                }

                // Preserve the original DateAjoutMovie
                movie.DateAjoutMovie = existingMovie.DateAjoutMovie;

                _context.Update(movie);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.Id))
                    return NotFound();
                throw;
            }
            catch (Exception ex)
            {
                ViewBag.Errors = new List<string> { 
                    $"Erreur lors de la modification: {ex.Message}",
                    ex.InnerException?.Message
                };
                return View(movie);
            }
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var movie = await _context.Movies
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound();

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var movie = await _context.Movies.FindAsync(id);
                if (movie != null)
                {
                    // Delete associated image file if it exists
                    if (!string.IsNullOrEmpty(movie.ImageFile))
                    {
                        var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", movie.ImageFile);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    _context.Movies.Remove(movie);
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

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}