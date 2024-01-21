using Core.Domain;
using Core.DomainServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class KantineEFRepo : IKantineRepo
    {
        private readonly FoodWasteDbContext _context;
        public KantineEFRepo(FoodWasteDbContext context)
        {
            _context = context;
        }

        public Kantine GetKantineById(int id)
        {
            return _context.Kantines.First(x => x.Id == id);
        }

        public IEnumerable<Kantine> GetKantines()
        {
            return _context.Kantines.ToList();
        }
    }
}
