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
        private readonly List<Product> _products = new List<Product>();

        public Task<bool> Add(Product product)
        {
            product.ProductId = Guid.NewGuid();

            _products.Add(product);

            return Task.FromResult(true);
        }

        public Task<bool> Delete(Guid productId)
        {
            var product = _products.FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                product.IsDeleted = 1;

                product.UpdatedAt = DateTime.Now;

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<IEnumerable<Product>> GetAll()
        {
            var products = _products.Where(p => p.IsDeleted == 0).ToList();

            return Task.FromResult((IEnumerable<Product>)products);
        }

        public Task<Product> GetById(Guid productId)
        {
            var product = _products.FirstOrDefault(p => p.ProductId == productId && p.IsDeleted == 0);

            return Task.FromResult(product);
        }

        public Task<Product> Update(Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.ProductId == product.ProductId && p.IsDeleted == 0);

            if (existingProduct != null)
            {
                existingProduct.ProductName = product.ProductName;

                existingProduct.ProductPrice = product.ProductPrice;

                existingProduct.Inventory = product.Inventory;

                existingProduct.UpdatedAt = DateTime.Now;

                return Task.FromResult(existingProduct);
            }
            return Task.FromResult<Product>(null);
        }
    }
}
