using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wakeDomain.Domain.Models;

namespace wakeDomain.Domain.Interfaces.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetById(Guid productid);
        Task<bool> Add(Product product);
        Task<Product> Update(Product product);
        Task<bool> Delete(Guid productid);
    }
}
