using MoviesCrudApp.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesCrudApp.Services
{
    /// <summary>
    /// Service that demonstrates Repository Pattern usage
    /// </summary>
    public interface IRepositoryDemoService
    {
        Task<int> GetTotalMoviesAsync();
        Task<int> GetTotalCustomersAsync();
        Task<int> GetTotalGenresAsync();
        Task<List<string>> GetGenreNamesAsync();
        Task<List<string>> GetCustomerNamesAsync();
    }

    public class RepositoryDemoService : IRepositoryDemoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RepositoryDemoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> GetTotalMoviesAsync()
        {
            return await _unitOfWork.Movies.CountAsync();
        }

        public async Task<int> GetTotalCustomersAsync()
        {
            return await _unitOfWork.Customers.CountAsync();
        }

        public async Task<int> GetTotalGenresAsync()
        {
            return await _unitOfWork.Genres.CountAsync();
        }

        public async Task<List<string>> GetGenreNamesAsync()
        {
            var genres = await _unitOfWork.Genres.GetAllAsync();
            var names = new List<string>();
            foreach (var genre in genres)
            {
                names.Add(genre.Name);
            }
            return names;
        }

        public async Task<List<string>> GetCustomerNamesAsync()
        {
            var customers = await _unitOfWork.Customers.GetAllAsync();
            var names = new List<string>();
            foreach (var customer in customers)
            {
                names.Add($"{customer.FirstName} {customer.LastName}");
            }
            return names;
        }
    }
}
