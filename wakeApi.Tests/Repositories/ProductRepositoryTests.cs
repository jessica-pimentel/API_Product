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
        private readonly ProductContext _context;
        private readonly ProductRepository _productRepository;

        public ProductRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ProductContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ProductContext(options);
            _productRepository = new ProductRepository(_context);
        }

        [Fact(DisplayName = "Deve Permitir Adicionar Produto Com Preco Positivo")]
        public async Task DevePermitirAdicionarProdutoComPrecoPositivo()
        {
            var product = new Product { ProductName = "produto teste", ProductPrice = 10m };

            var result = await _productRepository.Add(product);

            Assert.True(result);
            Assert.Equal(1, _context.Products.Count());
        }

        [Fact(DisplayName = "Deve Retornar Todos os Produtos")]
        public async Task DeveRetornarTodosOsProdutos()
        {
            var products = new List<Product>
            {
                new Product { ProductName = "Product 1", ProductPrice = 10m },
                new Product { ProductName = "Product 2", ProductPrice = 20m }
            };
            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();

            var result = await _productRepository.GetAll();

            Assert.Equal(products.Count, result.Count());
        }

        [Fact(DisplayName = "Deve Retornar Produto Não Encontrado quando ProductId não encontrado")]
        public async Task GetById_ProductNotFound_ShouldThrowKeyNotFoundException()
        {
            var productId = Guid.NewGuid();

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _productRepository.GetById(productId));
            Assert.Equal("Produto não encontrado.", exception.Message);
        }

        [Fact(DisplayName = "Deve Fazer Atualização Se Preco positivo e valido")]
        public async Task DeveFazerAtualizacaoSePrecoPositivoEValido()
        {
            var product = new Product { ProductName = "produto teste", ProductPrice = 10m };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            product.ProductPrice = 20m;

            var result = await _productRepository.Update(product);

            Assert.Equal(20m, result.ProductPrice);
        }

        [Fact(DisplayName = "Deve Retornar Verdadeiro Quando Produto Deletado Com Sucesso")]
        public async Task DeveRetornarVerdadeiroQuandoProdutoDeletadoComSucesso()
        {
            var product = new Product { ProductName = "produto teste", ProductPrice = 10m };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var result = await _productRepository.Delete(product.ProductId);

            Assert.True(result);
            Assert.Equal(0, _context.Products.Count(p => p.IsDeleted == 0));
        }
    }
}
