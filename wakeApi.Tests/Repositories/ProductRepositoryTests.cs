using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wakeDomain.Domain.Models;
using wakeInfra.Infra.Context;
using wakeInfra.Infra.Repository;

namespace wakeApi.Tests.Repositories
{
    public class ProductRepositoryTests
    {
        private readonly DbContextOptions<ProductContext> _options;

        public ProductRepositoryTests()
        {
            
            _options = new DbContextOptionsBuilder<ProductContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }
        private ProductContext CreateContext()
        {
            var context = new ProductContext(_options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }

        [Fact(DisplayName = "Deve Permitir Adicionar Produto Com Preco Positivo")]
        public async Task DevePermitirAdicionarProdutoComPrecoPositivo()
        {
            
            var product = new Product { ProductName = "produto teste", ProductPrice = 10m };
            using var context = CreateContext();
            var repository = new ProductRepository(context);

            var result = await repository.Add(product);
            
            Assert.True(result);
            Assert.Equal(1, context.Products.Count());
        }

        [Fact(DisplayName = "Deve Retornar Todos os Produtos")]
        public async Task DeveRetornarTodosOsProdutos()
        {
            var products = new List<Product>
            {
                new Product { ProductName = "Product 1", ProductPrice = 10m },
                new Product { ProductName = "Product 2", ProductPrice = 20m }
            };

            using var context = CreateContext();
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();

            var repository = new ProductRepository(context);
            
            var result = await repository.GetAll();

            Assert.Equal(products.Count, result.Count());
        }

        [Fact(DisplayName = "Deve Retornar Produto Não Encontrado quando ProductId não encontrado")]
        public async Task GetById_ProductNotFound_ShouldThrowKeyNotFoundException()
        {
            var productId = Guid.NewGuid();
            using var context = CreateContext();
            var repository = new ProductRepository(context);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => repository.GetById(productId));
            Assert.Equal("Produto não encontrado.", exception.Message);
        }

        [Fact(DisplayName = "Deve Fazer Atualização Se Preco positivo e valido")]
        public async Task DeveFazerAtualizacaoSePrecoPositivoEValido()
        {
            var product = new Product { ProductName = "produto teste", ProductPrice = 10m };
            using var context = CreateContext();
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            var repository = new ProductRepository(context);
            product.ProductPrice = 20m;

            var result = await repository.Update(product);

            Assert.Equal(20m, result.ProductPrice);
        }

        [Fact(DisplayName = "Deve Retornar Verdadeiro Quando Produto Deletado Com Sucesso")]
        public async Task DeveRetornarVerdadeiroQuandoProdutoDeletadoComSucesso()
        {
            var product = new Product { ProductName = "produto teste", ProductPrice = 10m };
            using var context = CreateContext();
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            var repository = new ProductRepository(context);

            var result = await repository.Delete(product.ProductId);

            Assert.True(result);
            Assert.Equal(0, context.Products.Count(p => p.IsDeleted == 0));
        }
    }
}
