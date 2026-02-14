using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesCrudApp.Models
{
    public class MembershipType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom du type d'adhésion est requis")]
        [StringLength(50, ErrorMessage = "Le nom ne peut pas dépasser 50 caractères")]
        [Display(Name = "Nom")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Le taux de réduction est requis")]
        [Range(0, 100, ErrorMessage = "Le taux de réduction doit être entre 0 et 100")]
        [Display(Name = "Taux de Réduction (%)")]
        public decimal DiscountRate { get; set; }

        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        // Navigation property
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
