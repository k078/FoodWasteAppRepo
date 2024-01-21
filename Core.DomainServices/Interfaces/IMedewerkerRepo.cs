using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Interfaces
{
    public interface IMedewerkerRepo
    {
        IEnumerable<Medewerker> GetMedewerkers();
        Medewerker GetMedewerkerById(int Id);
        Medewerker GetMedewerkerByEmail(string Email);
    }
}
