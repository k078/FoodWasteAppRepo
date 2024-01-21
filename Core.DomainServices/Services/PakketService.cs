using Core.Domain;
using Core.DomainServices.Interfaces;

namespace Core.DomainServices.Services
{
    public class PakketService : IPakketService
    {
        private readonly IPakketRepo _pakketRepo;
        private readonly IKantineRepo _kantineRepo;
        private readonly IStudentRepo _studentRepo;
        public PakketService(IPakketRepo pakketRepo, IStudentRepo studentRepo, IKantineRepo kantineRepo)
        {
            _pakketRepo = pakketRepo;
            _studentRepo = studentRepo;
            _kantineRepo = kantineRepo;
        }
        public PakketService(IPakketRepo repo)
        {
            _pakketRepo = repo;
        }
        public IEnumerable<Pakket> GetPakketten() => _pakketRepo.GetPakketten();

        public bool reserveerPakket(int pakketId, int studentId)
        {
            var pakket = _pakketRepo.GetPakketById(pakketId);
            if (pakket.gereserveerd != null)
            {
                return false;
            }
            _pakketRepo.ReserveerPakket(studentId, pakketId);
            return true;
        }

        public IEnumerable<Pakket> GetBeschikbarePakketten()
        {
            return _pakketRepo.GetBeschikbarePakketten();
        }

        public IEnumerable<Pakket> GetGereserveerdePakketten(int studentId)
        {
            return _pakketRepo.GetGereserveerdePakketten(studentId);
        }

        public Pakket GetPakketById(int Id)
        {
            return _pakketRepo.GetPakketById(Id);
        }

        public Pakket AddPakket(Pakket pakket)
        {
            if (pakket.pickup > pakket.pickUpMax)
            {
                throw new ArgumentException("De maximale ophaaltijd moet na de eerste ophaalmogelijkheid zijn!");
            }

            if (pakket.pickup > DateTime.Now.AddDays(2))
            {
                throw new ArgumentException("Er mag geen pakket toegevoegd worden wat pas over 2 dagen op te halen is!");
            }

            if (pakket.producten == null || !pakket.producten.Any())
            {
                throw new ArgumentException("Er moet minimaal een product toegevoeg worden aan een pakket!");
            }

            return _pakketRepo.AddPakket(pakket);
        }

        public void DeletePakket(int Id)
        {
            _pakketRepo.DeletePakket(Id);
        }

        public Pakket UpdatePakket(Pakket pakket)
        {
            if (pakket.pickup > pakket.pickUpMax)
            {
                throw new ArgumentException("De maximale ophaaltijd moet na de eerste ophaalmogelijkheid zijn!");
            }

            if (pakket.pickup > DateTime.Now.AddDays(2))
            {
                throw new ArgumentException("Er mag geen pakket toegevoegd worden wat pas over 2 dagen op te halen is!");
            }

            if (pakket.producten == null || !pakket.producten.Any())
            {
                throw new ArgumentException("Er moet minimaal een product toegevoeg worden aan een pakket!");
            }
            _pakketRepo.UpdatePakket(pakket);
            return pakket;
        }

        public void DeleteProductenNieuwPakket(int pakketId, int productId)
        {
            _pakketRepo.DeleteProductenNieuwPakket(pakketId, productId);
        }

        public void AddProducten(int pakketId, int productId)
        {
            _pakketRepo.AddProducten(pakketId, productId);
        }

        public void VerwijderVerlopenPakketten(DateTime time)
        {
            _pakketRepo.VerwijderVerlopenPakketten(time);
        }

        public IEnumerable<Product> GetPakketProducten(int pakketId)
        {
            return _pakketRepo.GetPakketProducten(pakketId);
        }

        public IEnumerable<Pakket> GetAlleGereserveerdePakketten()
        {
            return _pakketRepo.GetAlleGereserveerdePakketten();
        }
    }
}