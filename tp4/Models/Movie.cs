using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesCrudApp.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre est requis")]
        [StringLength(100, ErrorMessage = "Le titre ne peut pas dépasser 100 caractères")]
        public string Title { get; set; }

        [Required(ErrorMessage = "La description est requise")]
        [StringLength(500, ErrorMessage = "La description ne peut pas dépasser 500 caractères")]
        public string Description { get; set; }

        [Required(ErrorMessage = "La date de sortie est requise")]
        [DataType(DataType.Date)]
        [Display(Name = "Date de Sortie")]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "La durée est requise")]
        [Range(1, 500, ErrorMessage = "La durée doit être entre 1 et 500 minutes")]
        [Display(Name = "Durée (minutes)")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "La note est requise")]
        [Range(0.0, 10.0, ErrorMessage = "La note doit être entre 0.0 et 10.0")]
        [Display(Name = "Note")]
        public decimal Rating { get; set; }

        // Foreign Key for Genre
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        // Navigation property for Genre
        [ForeignKey("GenreId")]
        public virtual Genre Genre { get; set; }

        // New properties for file upload and date
        [StringLength(255)]
        [Display(Name = "Image")]
        public string ImageFile { get; set; }

        [Display(Name = "Date d'Ajout")]
        [DataType(DataType.DateTime)]
        public DateTime DateAjoutMovie { get; set; } = DateTime.Now;

        [Range(0, 100, ErrorMessage = "Le stock doit être entre 0 et 100")]
        [Display(Name = "Stock")]
        public int Stock { get; set; }
    }
}
