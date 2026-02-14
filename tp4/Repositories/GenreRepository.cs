using Microsoft.EntityFrameworkCore;
using MoviesCrudApp.Data;
using MoviesCrudApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesCrudApp.Repositories
{
    /// <summary>
    /// Genre Repository Implementation - genre-specific data access logic
    /// </summary>
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        private readonly ApplicationDbContext _context;

        public GenreRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Genre>> GetAllGenresWithMoviesAsync()
        {
            return await _context.Genres
                .Include(g => g.Movies)
                .ToListAsync();
        }

        public async Task<Genre> GetGenreWithMostMoviesAsync()
        {
            return await _context.Genres
                .Include(g => g.Movies)
                .OrderByDescending(g => g.Movies.Count())
                .FirstOrDefaultAsync();
        }

        public async Task<List<Genre>> GetTopGenresWithMostMoviesAsync(int count)
        {
            return await _context.Genres
                .Include(g => g.Movies)
                .OrderByDescending(g => g.Movies.Count())
                .Take(count)
                .ToListAsync();
        }
    }
}
