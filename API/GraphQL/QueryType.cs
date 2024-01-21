using Core.DomainServices.Interfaces;
using Core.Domain;

namespace API.GraphQL
{
    public class QueryType
    {
        [UsePaging]
        public IEnumerable<Pakket> pakketen([Service]IPakketRepo pakketRepo) =>
            pakketRepo.GetPakketten().AsQueryable();

    }
}
