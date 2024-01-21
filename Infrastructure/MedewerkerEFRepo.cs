using Core.Domain;
using Core.DomainServices.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class MedewerkerEFRepo : IMedewerkerRepo
    {
        private readonly FoodWasteDbContext _context;
        public MedewerkerEFRepo(FoodWasteDbContext context)
        {
            _context = context; 
        }
        public Medewerker GetMedewerkerByEmail(string Email)
        {
            return _context.Medewerkers.FirstOrDefault(m=>m.email == Email);
        }

        public Medewerker GetMedewerkerById(int Id)
        {
            return _context.Medewerkers.First(m => m.Id == Id);
        }

        public IEnumerable<Medewerker> GetMedewerkers()
        {
            return _context.Medewerkers.ToList();
        }
    }
}
