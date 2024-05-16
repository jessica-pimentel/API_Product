using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wakeDomain.Domain.Interfaces.Repository;
using wakeDomain.Domain.Interfaces.Service;
using wakeDomain.Domain.Models;

namespace wakeDomain.Domain.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<bool> Add(Product product)
        {
            if (product.ProductPrice < 0m)
                throw new ArgumentException("O valor do produto não pode ser negativo.");

            return await _productRepository.Add(product);
        }

        public async Task<bool> Delete(Guid productId)
        {
            return await _productRepository.Delete(productId);
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _productRepository.GetAll();
        }

        public async Task<Product> GetById(Guid productId)
        {
            return await _productRepository.GetById(productId);
        }

        public async Task<Product> Update(Product product)
        {
            if (product.ProductPrice < 0m)
                throw new ArgumentException("O valor do produto não pode ser negativo.");

            return await _productRepository.Update(product);
        }

        public async Task<IEnumerable<Product>> SearchByName(string name)
        {
            return await _productRepository.SearchByName(name);
        }

        public async Task<IEnumerable<Product>> OrderByType(string type)
        {
            return await _productRepository.OrderByType(type);
        }
    }
}
