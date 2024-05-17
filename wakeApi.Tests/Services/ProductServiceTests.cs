using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wakeDomain.Domain.Interfaces.Repository;
using wakeDomain.Domain.Interfaces.Service;
using wakeDomain.Domain.Models;
using wakeDomain.Domain.Service;

namespace wakeApi.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly IProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _productService = new ProductService(_productRepositoryMock.Object);
        }

        [Fact(DisplayName = "Nao Deve Permitir Adicionar Produto Com Preco Negativo")]
        public async Task NaoDevePermitirAdicionarProdutoComPrecoNegativo()
        {
            var product = new Product { ProductName = "produto teste", ProductPrice = -1m };

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _productService.Add(product));

            Assert.Equal("O valor do produto não pode ser negativo.", exception.Message);
        }

        [Fact(DisplayName = "Deve Permitir Adicionar Produto Com Preco Positivo")]
        public async Task DevePermitirAdicionarProdutoComPrecoPositivo()
        {
            var product = new Product { ProductName = "produto teste", ProductPrice = 10m };

            _productRepositoryMock.Setup(repo => repo.Add(It.IsAny<Product>())).ReturnsAsync(true);

            var result = await _productService.Add(product);

            Assert.True(result);
        }

        [Fact(DisplayName = "Deve Retornar Todos os Produtos")]
        public async Task DeveRetornarTodosoOProdutos()
        {
            var products = new List<Product>
            {
                new Product { ProductName = "Product 1", ProductPrice = 10m },
                new Product { ProductName = "Product 2", ProductPrice = 20m }
            };
            _productRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(products);

            var result = await _productService.GetAll();

            Assert.Equal(products.Count, result.Count());
        }

        [Fact(DisplayName = "Deve Retornar Produto Não Encontrado quando ProductId não encontrado")]
        public async Task GetById_ProductNotFound_ShouldReturnNull()
        {
            var productId = Guid.NewGuid();
            _productRepositoryMock.Setup(repo => repo.GetById(productId)).ReturnsAsync((Product)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.GetById(productId));
            Assert.Equal("Produto não encontrado.", exception.Message);
        }

        [Fact(DisplayName = "Nao Deve Fazer Atualização Se Preco Negativo")]
        public async Task NaoDeveFazerAtualizacaoSePrecoNegativo()
        {
            var product = new Product { ProductId = Guid.NewGuid(), ProductName = "produto teste", ProductPrice = -1m };

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _productService.Update(product));
            Assert.Equal("O valor do produto não pode ser negativo.", exception.Message);
        }

        [Fact(DisplayName = "Deve Fazer Atualização Se Preco positivo e valido")]
        public async Task DeveFazerAtualizacaoSePrecoPositivoEValido()
        {
            var product = new Product { ProductId = Guid.NewGuid(), ProductName = "produto teste", ProductPrice = 10m };

            _productRepositoryMock.Setup(repo => repo.Update(It.IsAny<Product>())).ReturnsAsync(product);

            var result = await _productService.Update(product);

            Assert.Equal(product.ProductName, result.ProductName);

            Assert.Equal(product.ProductPrice, result.ProductPrice);
        }
    }
}
