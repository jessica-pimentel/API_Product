using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wakeDomain.Domain.Models;

namespace wakeInfra.Infra.Context.Global
{
    public static class SeedInitializeDataBase
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ProductContext(
                serviceProvider.GetRequiredService<DbContextOptions<ProductContext>>()))
            {
                if (context.Products.Any())
                    return;   

                var products = new List<Product>
                {
                    new Product { ProductName = "Produto 1", ProductPrice = 5.95m, Inventory = 10, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsDeleted = 0},
                    new Product { ProductName = "Produto 2", ProductPrice = 6.35m, Inventory = 20, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsDeleted = 0},
                    new Product { ProductName = "Produto 3", ProductPrice = 2.50m, Inventory = 30, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsDeleted = 0},
                    new Product { ProductName = "Produto 4", ProductPrice = 5.00m, Inventory = 40, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsDeleted = 0},
                    new Product { ProductName = "Produto 5", ProductPrice = 1.99m, Inventory = 50, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsDeleted = 0}
                };

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}
