using Microsoft.AspNetCore.Mvc;
using MoviesCrudApp.Repositories;
using System.Threading.Tasks;

namespace MoviesCrudApp.Controllers
{
    public class RepositoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RepositoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Repository
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalMovies = await _unitOfWork.Movies.CountAsync();
            ViewBag.TotalCustomers = await _unitOfWork.Customers.CountAsync();
            ViewBag.TotalGenres = await _unitOfWork.Genres.CountAsync();
            ViewBag.TotalMembershipTypes = await _unitOfWork.MembershipTypes.CountAsync();

            return View();
        }

        // GET: Repository/Movies
        public async Task<IActionResult> Movies()
        {
            var movies = await _unitOfWork.Movies.GetAllMoviesWithGenreAsync();
            return View(movies);
        }

        // GET: Repository/Customers
        public async Task<IActionResult> Customers()
        {
            var customers = await _unitOfWork.Customers.GetAllCustomersWithMembershipAsync();
            return View(customers);
        }

        // GET: Repository/Genres
        public async Task<IActionResult> Genres()
        {
            var genres = await _unitOfWork.Genres.GetAllGenresWithMoviesAsync();
            return View(genres);
        }

        // GET: Repository/TopGenres
        public async Task<IActionResult> TopGenres()
        {
            var topGenres = await _unitOfWork.Genres.GetTopGenresWithMostMoviesAsync(3);
            return View(topGenres);
        }

        // GET: Repository/MoviesWithStock
        public async Task<IActionResult> MoviesWithStock()
        {
            var movies = await _unitOfWork.Movies.GetMoviesWithStockAsync();
            return View(movies);
        }

        // GET: Repository/NewsletterSubscribers
        public async Task<IActionResult> NewsletterSubscribers()
        {
            var customers = await _unitOfWork.Customers.GetNewsletterSubscribersAsync();
            return View(customers);
        }

        // GET: Repository/HighDiscountCustomers
        public async Task<IActionResult> HighDiscountCustomers()
        {
            var customers = await _unitOfWork.Customers.GetCustomersWithHighDiscountAsync();
            return View(customers);
        }

        // GET: Repository/SearchMovies?searchTerm=action
        public async Task<IActionResult> SearchMovies(string searchTerm)
        {
            ViewBag.SearchTerm = searchTerm;
            if (string.IsNullOrEmpty(searchTerm))
            {
                return View(new System.Collections.Generic.List<Models.Movie>());
            }

            var movies = await _unitOfWork.Movies.SearchMoviesAsync(searchTerm);
            return View(movies);
        }

        // GET: Repository/SearchCustomers?searchTerm=john
        public async Task<IActionResult> SearchCustomers(string searchTerm)
        {
            ViewBag.SearchTerm = searchTerm;
            if (string.IsNullOrEmpty(searchTerm))
            {
                return View(new System.Collections.Generic.List<Models.Customer>());
            }

            var customers = await _unitOfWork.Customers.SearchCustomersAsync(searchTerm);
            return View(customers);
        }
    }
}
