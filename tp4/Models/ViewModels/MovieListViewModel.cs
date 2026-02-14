using MoviesCrudApp.Models;
using System.Collections.Generic;

namespace MoviesCrudApp.Models.ViewModels
{
    public class MovieListViewModel
    {
        public List<Movie> Movies { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public string SearchString { get; set; }
    }
}
