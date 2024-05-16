using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wakeDomain.Domain.Interfaces.Service;
using wakeDomain.Domain.Models;

namespace wakeDomain.Domain.Service
{
    public class ProductService : IProductService
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
