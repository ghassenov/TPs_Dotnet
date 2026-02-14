using Microsoft.AspNetCore.Mvc;
using MoviesCrudApp.Services;
using System.Threading.Tasks;

namespace MoviesCrudApp.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IMovieService _movieService;

        public ServicesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // GET: Services
        public IActionResult Index()
        {
            return View();
        }

        // GET: Services/GetActionMoviesWithStock
        public async Task<IActionResult> ActionMoviesWithStock()
        {
            var movies = await _movieService.GetActionMoviesWithStockAsync();
            ViewBag.Title = "Films Action avec Stock > 0";
            ViewBag.Description = "Affiche les films de genre Action avec un stock disponible";
            return View("ServiceResult", movies);
        }

        // GET: Services/GetMoviesSorted
        public async Task<IActionResult> MoviesSorted()
        {
            var movies = await _movieService.GetMoviesSortedByReleaseDateAndTitleAsync();
            ViewBag.Title = "Films Triés par Date de Sortie et Titre";
            ViewBag.Description = "Affiche tous les films ordonnés par date de sortie, puis par titre";
            return View("ServiceResult", movies);
        }

        // GET: Services/GetTotalMovieCount
        public async Task<IActionResult> TotalMovieCount()
        {
            var count = await _movieService.GetTotalMovieCountAsync();
            ViewBag.Title = "Nombre Total de Films";
            ViewBag.Description = "Compte le nombre total de films dans la base de données";
            ViewBag.Count = count;
            return View("TotalCount");
        }

        // GET: Services/GetSubscribedCustomers
        public async Task<IActionResult> SubscribedCustomersHighDiscount()
        {
            var customers = await _movieService.GetSubscribedCustomersWithHighDiscountAsync();
            ViewBag.Title = "Clients Abonnés à la Newsletter avec Réduction > 10%";
            ViewBag.Description = "Affiche les clients abonnés à la newsletter avec un taux de réduction supérieur à 10%";
            return View("CustomerResult", customers);
        }

        // GET: Services/GetMoviesWithGenres
        public async Task<IActionResult> MoviesWithGenres()
        {
            var moviesGenres = await _movieService.GetMoviesWithGenresAsync();
            ViewBag.Title = "Films avec leurs Genres";
            ViewBag.Description = "Affiche la liste des films avec leur genre au format 'Titre - Genre'";
            return View("MoviesGenreResult", moviesGenres);
        }

        // GET: Services/GetTop3Genres
        public async Task<IActionResult> Top3GenresWithMostMovies()
        {
            var genreCounts = await _movieService.GetTop3GenresWithMostMoviesAsync();
            ViewBag.Title = "Top 3 Genres avec le Plus de Films";
            ViewBag.Description = "Affiche les 3 genres ayant le plus grand nombre de films";
            return View("GenreCountResult", genreCounts);
        }
    }
}
