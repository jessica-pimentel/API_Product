using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wakeDomain.Domain.Interfaces.Repository;
using wakeDomain.Domain.Models;
using wakeInfra.Infra.Context;

namespace wakeInfra.Infra.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;
        public ProductRepository(ProductContext context)
        {
            _context = context;   
        }

        public async Task<bool> Add(Product product)
        {
            product.ProductId = Guid.NewGuid();
            product.CreatedAt = DateTime.Now;
            product.UpdatedAt = DateTime.Now;

            await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid productId)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
                throw new KeyNotFoundException("Produto não encontrado.");

            product.IsDeleted = 1;
            product.UpdatedAt = DateTime.Now;

            _context.Products.Update(product);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.Where(p => p.IsDeleted == 0).ToListAsync();
        }

        public async Task<Product> GetById(Guid productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId && p.IsDeleted == 0);

            if (product == null)
                throw new KeyNotFoundException("Produto não encontrado.");

            return product;
        }

        public async Task<Product> Update(Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.ProductId);

            if (existingProduct == null || existingProduct.IsDeleted != 0)
                throw new KeyNotFoundException("Produto não encontrado.");

            existingProduct.ProductName = product.ProductName;
            existingProduct.ProductPrice = product.ProductPrice;
            existingProduct.Inventory = product.Inventory;
            existingProduct.UpdatedAt = DateTime.Now;

            _context.Products.Update(existingProduct);

            await _context.SaveChangesAsync();

            return existingProduct;
        }

        public async Task<IEnumerable<Product>> SearchByName(string name)
        {
            return await _context.Products.Where(p => p.ProductName.Contains(name) && p.IsDeleted == 0).ToListAsync();
        }

        public async Task<IEnumerable<Product>> OrderByType(string type)
        {
            IQueryable<Product> query = _context.Products.Where(p => p.IsDeleted == 0);

            switch (type.ToLower())
            {
                case "name":
                    query = query.OrderBy(p => p.ProductName);
                    break;
                case "price":
                    query = query.OrderBy(p => p.ProductPrice);
                    break;
                case "inventory":
                    query = query.OrderBy(p => p.Inventory);
                    break;
                default:
                    throw new ArgumentException("Tipo de ordenação não suportado.");
            }

            return await query.ToListAsync();
        }
    }
}
