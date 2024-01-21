using Core.Domain;
using Core.DomainServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ProductEFRepo : IProductRepo
    {
        private readonly FoodWasteDbContext _context;
        public ProductEFRepo(FoodWasteDbContext context)
        {
            _context = context;
        }

        public Product GetProductById(int Id)
        {
            return _context.Producten.First(x => x.Id == Id);
        }

        public IEnumerable<Product> GetProducten()
        {
            return _context.Producten.ToList();
        }
        public Product getProductById(int Id)
        {
            return _context.Producten.First(x => x.Id == Id);
        }
    }
}
