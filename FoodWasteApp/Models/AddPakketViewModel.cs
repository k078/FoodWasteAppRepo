using Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace FoodWasteApp.Models
{
    public class AddPakketViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Geef een naam mee aan de maaltijd")]
        public string titel { get; set; }
        public ICollection<Product>? producten { get; set; }
        [Required (ErrorMessage = "Selecteer minimaal 1 product!")]
        public List<int> selectedProducten { get; set; }
        public List<ProductCheckModel> productchecks { get; set; } = new List<ProductCheckModel>();
        public Stad stad { get; set; }
        public Kantine? kantine { get; set; }
        public DateTime pickup { get; set; }
        public DateTime pickUpMax { get; set; }
        public bool volwassen { get; set; }
        [Required]
        [Range(0.50, 99, ErrorMessage = "De prijs moet minimaal €0,50 bedragen")]
        public double prijs { get; set; }
        public Maaltijd maaltijd { get; set; }
        public Student? reserveerder { get; set; }
        public bool warm { get; set; } = false;
    }
}
