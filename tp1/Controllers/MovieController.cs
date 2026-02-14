using Microsoft.AspNetCore.Mvc;
using tp1.Models;
using tp1.ViewModels;
using System.Collections.Generic;

namespace tp1.Controllers
{
    public class MovieController : Controller
    {
        public IActionResult Index()
        {
            var movies = new List<Movie>
            {
                new Movie { Id = 1, Name = "Inception" },
                new Movie { Id = 2, Name = "Interstellar" },
                new Movie { Id = 3, Name = "The Dark Knight" }
            };

            return View(movies);
        }

        public IActionResult Edit(int id)
        {
            return Content("Test Id: " + id);
        }

        [Route("Movie/released/{year:int}/{month:int}")]
        public IActionResult ByRelease(int year, int month)
        {
            return Content($"Movies released in {month}/{year}");
        }

        public IActionResult CustomerMovies(int id)
        {
            var customer = new Customer { Id = id, Name = "Omar" };

            var movies = new List<Movie>
            {
                new Movie { Id = 1, Name = "Matrix" },
                new Movie { Id = 2, Name = "Avatar" },
                new Movie { Id = 3, Name = "Gladiator" }
            };

            var vm = new MovieCustomerViewModel
            {
                Customer = customer,
                Movies = movies
            };

            return View(vm);
        }

        public IActionResult Details(int id)
        {
            var movie = new Movie
            {
                Id = id,
                Name = "Movie " + id
            };

            return View(movie);
        }
    }
}

