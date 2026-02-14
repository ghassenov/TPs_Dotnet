using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesCrudApp.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom du genre est requis")]
        [StringLength(50, ErrorMessage = "Le nom ne peut pas dépasser 50 caractères")]
        [Display(Name = "Nom du Genre")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "La description ne peut pas dépasser 500 caractères")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        // Navigation property
        public virtual ICollection<Movie> Movies { get; set; }
    }
}
