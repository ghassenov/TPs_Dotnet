using MoviesCrudApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesCrudApp.Repositories
{
    /// <summary>
    /// Genre Repository Interface - extends generic repository with genre-specific operations
    /// </summary>
    public interface IGenreRepository : IGenericRepository<Genre>
    {
        /// <summary>
        /// Get all genres with their movies included
        /// </summary>
        Task<List<Genre>> GetAllGenresWithMoviesAsync();

        /// <summary>
        /// Get genre with most movies
        /// </summary>
        Task<Genre> GetGenreWithMostMoviesAsync();

        /// <summary>
        /// Get top genres with most movies
        /// </summary>
        Task<List<Genre>> GetTopGenresWithMostMoviesAsync(int count);
    }
}
