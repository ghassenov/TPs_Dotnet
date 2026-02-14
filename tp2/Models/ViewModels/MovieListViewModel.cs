using System.Collections.Generic;

namespace MoviesCrudApp.Models.ViewModels
{
    public class MovieListViewModel
    {
        public IEnumerable<Movie> Movies { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public string CurrentFilter { get; set; }
    }

    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
}
