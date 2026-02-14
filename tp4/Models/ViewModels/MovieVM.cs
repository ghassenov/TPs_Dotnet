using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MoviesCrudApp.Models.ViewModels
{
    public class MovieVM
    {
        public Movie Movie { get; set; } = new Movie();
        
        [Display(Name = "Photo")]
        public IFormFile? Photo { get; set; }
    }
}