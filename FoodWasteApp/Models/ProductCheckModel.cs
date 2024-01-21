using Core.Domain;

namespace FoodWasteApp.Models
{
    public class ProductCheckModel
    {
        public bool IsChecked { get; set; }
        int Id { get; set; }
        public string naam { get; set; }
        public Product product { get; set; }

    }
}
