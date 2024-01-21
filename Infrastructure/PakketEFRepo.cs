using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using Core.DomainServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class PakketEFRepo : IPakketRepo
    {
        private readonly FoodWasteDbContext _context;

        public PakketEFRepo(FoodWasteDbContext context)
        {
            _context = context;
        }

        public Pakket AddPakket(Pakket pakket)
        {
            _context.Pakketten.Add(pakket);
            _context.SaveChanges();
            return pakket;
        }

        public void AddProducten(int pakketId, int productId)
        {
            var pakket = _context.Pakketten.Include(p => p.producten)
                .FirstOrDefault(p => p.Id == pakketId);

            if (pakket != null)
            {
                var productToevoegen = _context.Producten.FirstOrDefault(p => p.Id == productId);
                if (productToevoegen != null && !pakket.producten.Any(p => p.Id == productId))
                {
                    pakket.producten.Add(productToevoegen);
                    _context.SaveChanges();
                }
            }
        }

        public void DeletePakket(int Id)
        {
            Pakket pakket = _context.Pakketten.Find(Id);

            if (pakket != null)
            {
                _context.Pakketten.Remove(pakket);
                _context.SaveChanges();
            }
        }

        public void DeleteProductenNieuwPakket(int pakketId, int productId)
        {
            var pakket = _context.Pakketten.Include(p => p.producten)
                .FirstOrDefault(p => p.Id == pakketId);

            if (pakket != null)
            {
                var productVerwijderen = _context.Producten.FirstOrDefault(p => p.Id == productId);
                if (productVerwijderen != null)
                {
                    pakket.producten.Remove(productVerwijderen);
                    _context.SaveChanges();
                }
            }
        }

        public IEnumerable<Pakket> GetAlleGereserveerdePakketten()
        {
            return _context.Pakketten
                .Include(p => p.producten)
                .Include(k => k.kantine)
                .Include(p => p.gereserveerd)
                .Where(p => p.gereserveerd != null)
                .ToList();
        }

        public IEnumerable<Pakket> GetBeschikbarePakketten()
        {
            return _context.Pakketten.Where(p => p.gereserveerd == null).ToList();
        }

        public IEnumerable<Pakket> GetGereserveerdePakketten(int studentId)
        {
            return _context.Pakketten
                .Include(p => p.producten)
                .Include(k => k.kantine)
                .Include(p => p.gereserveerd)
                .Where(p => p.gereserveerd != null && p.gereserveerd.Id == studentId)
                .OrderBy(p=>p.pickup)
                .ToList();
        }

        public Pakket GetPakketById(int Id)
        {
            return _context.Pakketten.FirstOrDefault(p => p.Id == Id);
        }

        public IEnumerable<Product> GetPakketProducten(int pakketId)
        {
            return _context.Pakketten
                .Where(p => p.Id == pakketId)
                .SelectMany(p => p.producten)
                .ToList();
        }

        public IEnumerable<Pakket> GetPakketten()
        {
            return _context.Pakketten
                .Include(p => p.producten)
                .Include(k => k.kantine)
                .Include(p => p.gereserveerd)
                .OrderBy(p=>p.pickup).ToList();
        }

        public Pakket ReserveerPakket(int studentId, int pakketId)
        {
            var pakket = _context.Pakketten.Include(p => p.producten).Include(p => p.gereserveerd).FirstOrDefault(p => p.Id == pakketId);
            if (pakket == null)
            {
                throw new NullReferenceException("Pakket not found");
            }

            if (pakket.gereserveerd != null)
            {
                throw new Exception("Pakket al gereserveerd");
            }

            var student = _context.Students.FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                throw new NullReferenceException("Student niet gevonden");
            }

            pakket.gereserveerd = student;
            _context.SaveChanges();

            return pakket;
        }


        public Pakket UpdatePakket(Pakket pakket)
        {
            if (pakket != null)
            {
                _context.Pakketten.Update(pakket);
                _context.SaveChanges();
            }
            return pakket;
        }

        public void VerwijderVerlopenPakketten(DateTime currentTime)
        {
            var verlopenPakketten = _context.Pakketten.Where(p => p.pickUpMax < currentTime).ToList();

            foreach (var pakket in verlopenPakketten)
            {
                _context.Pakketten.Remove(pakket);
            }

            _context.SaveChanges();
        }
    }
}
