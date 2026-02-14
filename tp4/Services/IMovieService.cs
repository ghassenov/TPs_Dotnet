using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesCrudApp.Services
{
    public interface IMovieService
    {
        /// <summary>
        /// Get all Action movies with stock greater than 0
        /// </summary>
        Task<List<MovieServiceDTO>> GetActionMoviesWithStockAsync();

        /// <summary>
        /// Get all movies ordered by ReleaseDate then Title
        /// </summary>
        Task<List<MovieServiceDTO>> GetMoviesSortedByReleaseDateAndTitleAsync();

        /// <summary>
        /// Get total count of all movies
        /// </summary>
        Task<int> GetTotalMovieCountAsync();

        /// <summary>
        /// Get all customers subscribed to newsletter with discount rate > 10%
        /// </summary>
        Task<List<CustomerServiceDTO>> GetSubscribedCustomersWithHighDiscountAsync();

        /// <summary>
        /// Get movies with their genres in format "Title - Genre"
        /// </summary>
        Task<List<MovieGenreDTO>> GetMoviesWithGenresAsync();

        /// <summary>
        /// Get top 3 genres with most movies count
        /// </summary>
        Task<List<GenreCountDTO>> GetTop3GenresWithMostMoviesAsync();
    }

    public class MovieServiceDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; }
        public decimal Rating { get; set; }
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public string ImageFile { get; set; }
        public int Stock { get; set; }
    }

    public class CustomerServiceDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int MembershipTypeId { get; set; }
        public string MembershipTypeName { get; set; }
        public decimal DiscountRate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsSubscribedToNewsletter { get; set; }
    }

    public class MovieGenreDTO
    {
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public string GenreName { get; set; }
        public string Display { get; set; } // Format: "Title - Genre"
    }

    public class GenreCountDTO
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public int MovieCount { get; set; }
    }
}
