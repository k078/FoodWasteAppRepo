using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Kantine  
    {
        public int Id { get; set; } 
        public string? naam { get; set; }
        public Stad stad { get;set; }
        public Locatie locatie { get;set; }
        public bool warm { get;set; }
    }
}
