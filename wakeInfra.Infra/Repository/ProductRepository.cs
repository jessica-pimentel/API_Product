using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wakeDomain.Domain.Interfaces.Repository;
using wakeDomain.Domain.Models;

namespace wakeInfra.Infra.Repository
{
    public class ProductRepository : IProductRepository
    {
        public Task<bool> Add(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid productid)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetById(Guid productid)
        {
            throw new NotImplementedException();
        }

        public Task<Product> Update(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
