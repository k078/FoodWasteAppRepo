using Core.Domain;

namespace Core.DomainServices.Interfaces
{
    public interface IPakketService
    {
        bool reserveerPakket(int pakketId, int studentId);
        IEnumerable<Pakket> GetPakketten();
        IEnumerable<Pakket> GetBeschikbarePakketten();
        IEnumerable<Pakket> GetGereserveerdePakketten(int studentId);
        Pakket GetPakketById(int Id);
        Pakket AddPakket(Pakket pakket);
        void DeletePakket(int Id);
        Pakket UpdatePakket(Pakket pakket);
        void DeleteProductenNieuwPakket(int pakketId, int productId);
        void AddProducten(int pakketId, int productId);
        void VerwijderVerlopenPakketten(DateTime time);
        IEnumerable<Product> GetPakketProducten(int pakketId);
        IEnumerable<Pakket> GetAlleGereserveerdePakketten();
    }
}
