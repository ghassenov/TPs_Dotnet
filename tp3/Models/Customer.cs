using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesCrudApp.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le prénom est requis")]
        [StringLength(50, ErrorMessage = "Le prénom ne peut pas dépasser 50 caractères")]
        [Display(Name = "Prénom")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Le nom de famille est requis")]
        [StringLength(50, ErrorMessage = "Le nom ne peut pas dépasser 50 caractères")]
        [Display(Name = "Nom")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide")]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Le numéro de téléphone n'est pas valide")]
        [StringLength(20)]
        [Display(Name = "Téléphone")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Le type d'adhésion est requis")]
        [Display(Name = "Type d'Adhésion")]
        public int MembershipTypeId { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date d'Inscription")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Foreign Key
        [ForeignKey("MembershipTypeId")]
        public virtual MembershipType MembershipType { get; set; }
    }
}
