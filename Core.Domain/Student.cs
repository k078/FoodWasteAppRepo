using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Student
    {
        public int Id { get; set; }
        public string naam { get; set; }
        public int studentnummer { get; set; }
        public string email { get; set; }
        public string stad { get; set; }
        public DateTime geboortedatum { get; set; }
    }
}
