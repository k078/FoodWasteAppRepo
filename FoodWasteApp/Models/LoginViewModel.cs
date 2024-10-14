using System.ComponentModel.DataAnnotations;

namespace FoodWasteApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "U moet een valide emailadres opgeven")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "U moet wachtwoord opgeven")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
