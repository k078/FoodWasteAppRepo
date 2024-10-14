using System;
using System.ComponentModel.DataAnnotations;

namespace FoodWasteApp.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "U moet een valide emailadres opgeven")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage ="U moet een wachtwoord opgeven")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "U moet nogmaals uw wachtwoord opgeven")]
        [DataType(DataType.Password)]
        [Display(Name = "Herhaal wachtwoord")]
        [Compare("Password", ErrorMessage = "Wachtwoorden komen niet overeen")]
        public string ConfirmPassword { get; set; } = null!;


    }
}
