using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Pakket
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Titel ontbreekt")]
        public string titel { get; set; }
        [Required(ErrorMessage = "Stad ontbreekt")]
        public Stad stad { get; set; }
        [Required(ErrorMessage = "Kantine ontbreekt")]
        public Kantine? kantine { get; set; }
        [Required(ErrorMessage = "Pickup ontbreekt")]   
        public DateTime pickup { get; set; }
        [Required(ErrorMessage = "PickupMax ontbreekt")]
        public DateTime pickUpMax { get; set; }
        [Required(ErrorMessage = "18+ ontbreekt")]
        public Boolean volwassen { get; set; }
        [Required(ErrorMessage = "Prijs ontbreekt")]
        public double prijs { get; set; }
        [Required(ErrorMessage = "Maaltijd type ontbreekt")]
        public Maaltijd maaltijd { get; set; }
        public Student? gereserveerd { get; set; }
        public ICollection<Product>? producten { get; set; }
        public bool warm { get; set; }
    }
}
