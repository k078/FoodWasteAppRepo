using Core.Domain;

namespace Core.DomainServices.Interfaces
{
    public interface IPakketRepo
    {
        IEnumerable<Pakket> GetPakketten();
        IEnumerable<Pakket> GetBeschikbarePakketten();
        IEnumerable<Pakket> GetGereserveerdePakketten(int studentId);
        IEnumerable<Pakket> GetAlleGereserveerdePakketten();
        Pakket GetPakketById(int Id);
        Pakket AddPakket(Pakket pakket);
        Pakket ReserveerPakket(int studentId, int pakketId);
        void DeletePakket(int Id);
        Pakket UpdatePakket(Pakket pakket);
        void DeleteProductenNieuwPakket(int pakketId, int productId);
        void AddProducten(int pakketId, int productId);
        void VerwijderVerlopenPakketten(DateTime time);
        IEnumerable<Product> GetPakketProducten(int pakketId);
    }
}