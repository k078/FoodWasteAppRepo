using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;

namespace Core.DomainServices.Interfaces
{
    public interface IProductRepo
    {
        IEnumerable<Product> GetProducten();
        Product GetProductById(int Id);

    }
}
