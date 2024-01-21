using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string naam { get; set; }
        public Boolean alcohol { get; set; }
        public string foto { get; set; }
        [JsonIgnore]
        public ICollection<Pakket> pakketten { get; set; } = null!;
    }
}
