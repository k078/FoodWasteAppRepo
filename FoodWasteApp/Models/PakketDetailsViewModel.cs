using Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace FoodWasteApp.Models
{
    public class PakketDetailsViewModel
    {
        public Pakket pakket { get; set; }
        public List<Product> producten { get; set; }
    }
}
