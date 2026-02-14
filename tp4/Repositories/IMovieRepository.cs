using MoviesCrudApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesCrudApp.Repositories
{
    /// <summary>
    /// Movie Repository Interface - extends generic repository with movie-specific operations
    /// </summary>
    public interface IMovieRepository : IGenericRepository<Movie>
    {
        /// <summary>
        /// Get all movies with their genre included
        /// </summary>
        Task<List<Movie>> GetAllMoviesWithGenreAsync();

        /// <summary>
        /// Get movies by genre
        /// </summary>
        Task<List<Movie>> GetMoviesByGenreAsync(int genreId);

        /// <summary>
        /// Get movies with stock greater than zero
        /// </summary>
        Task<List<Movie>> GetMoviesWithStockAsync();

        /// <summary>
        /// Search movies by title or description
        /// </summary>
        Task<List<Movie>> SearchMoviesAsync(string searchTerm);

        /// <summary>
        /// Get movies sorted by release date
        /// </summary>
        Task<List<Movie>> GetMoviesSortedByReleaseDateAsync();
    }
}
