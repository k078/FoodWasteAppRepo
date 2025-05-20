using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Medewerker
    {
        public int Id { get; set; }
        public string naam { get; set; }
        public string email { get; set; }   
        public int persooneelsNr { get; set; }
        public Kantine kantine { get; set; }
        public int kantineId {  get; set; }
    }
}
