using Microsoft.EntityFrameworkCore;
using MoviesCrudApp.Data;
using MoviesCrudApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesCrudApp.Services
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;

        public MovieService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all Action movies with stock greater than 0
        /// </summary>
        public async Task<List<MovieServiceDTO>> GetActionMoviesWithStockAsync()
        {
            var movies = await _context.Movies
                .Include(m => m.Genre)
                .Where(m => m.Genre.Name == "Action" && m.Stock > 0)
                .Select(m => new MovieServiceDTO
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    ReleaseDate = m.ReleaseDate,
                    Duration = m.Duration,
                    Rating = m.Rating,
                    GenreId = m.GenreId,
                    GenreName = m.Genre.Name,
                    ImageFile = m.ImageFile,
                    Stock = m.Stock
                })
                .ToListAsync();

            return movies;
        }

        /// <summary>
        /// Get all movies ordered by ReleaseDate then Title
        /// </summary>
        public async Task<List<MovieServiceDTO>> GetMoviesSortedByReleaseDateAndTitleAsync()
        {
            var movies = await _context.Movies
                .Include(m => m.Genre)
                .OrderBy(m => m.ReleaseDate)
                .ThenBy(m => m.Title)
                .Select(m => new MovieServiceDTO
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    ReleaseDate = m.ReleaseDate,
                    Duration = m.Duration,
                    Rating = m.Rating,
                    GenreId = m.GenreId,
                    GenreName = m.Genre.Name,
                    ImageFile = m.ImageFile,
                    Stock = m.Stock
                })
                .ToListAsync();

            return movies;
        }

        /// <summary>
        /// Get total count of all movies
        /// </summary>
        public async Task<int> GetTotalMovieCountAsync()
        {
            var count = await _context.Movies.CountAsync();
            return count;
        }

        /// <summary>
        /// Get all customers subscribed to newsletter with discount rate > 10%
        /// </summary>
        public async Task<List<CustomerServiceDTO>> GetSubscribedCustomersWithHighDiscountAsync()
        {
            var customers = await _context.Customers
                .Include(c => c.MembershipType)
                .Where(c => c.IsSubscribedToNewsletter && c.MembershipType.DiscountRate > 10)
                .Select(c => new CustomerServiceDTO
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    Phone = c.Phone,
                    MembershipTypeId = c.MembershipTypeId,
                    MembershipTypeName = c.MembershipType.Name,
                    DiscountRate = c.MembershipType.DiscountRate,
                    RegistrationDate = c.RegistrationDate,
                    IsSubscribedToNewsletter = c.IsSubscribedToNewsletter
                })
                .ToListAsync();

            return customers;
        }

        /// <summary>
        /// Get movies with their genres in format "Title - Genre"
        /// </summary>
        public async Task<List<MovieGenreDTO>> GetMoviesWithGenresAsync()
        {
            var moviesWithGenres = await _context.Movies
                .Include(m => m.Genre)
                .Select(m => new MovieGenreDTO
                {
                    MovieId = m.Id,
                    MovieTitle = m.Title,
                    GenreName = m.Genre.Name,
                    Display = m.Title + " - " + m.Genre.Name
                })
                .ToListAsync();

            return moviesWithGenres;
        }

        /// <summary>
        /// Get top 3 genres with most movies count
        /// </summary>
        public async Task<List<GenreCountDTO>> GetTop3GenresWithMostMoviesAsync()
        {
            var topGenres = await _context.Genres
                .AsNoTracking()
                .Select(g => new GenreCountDTO
                {
                    GenreId = g.Id,
                    GenreName = g.Name,
                    MovieCount = g.Movies.Count()
                })
                .OrderByDescending(g => g.MovieCount)
                .Take(3)
                .ToListAsync();

            return topGenres;
        }
    }
}
