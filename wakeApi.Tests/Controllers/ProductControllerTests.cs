using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wakeDomain.Domain.Interfaces.Service;
using wakeDomain.Domain.Models;

namespace wakeApi.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductController _productController;

        public ProductControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _productController = new ProductController(_productServiceMock.Object);
        }

        [Fact(DisplayName = "Deve Adicionar Produto Com Preco Positivo")]
        public async Task DeveAdicionarProdutoComPrecoPositivo()
        {
            var product = new Product { ProductName = "produto teste", ProductPrice = 10m };
            _productServiceMock.Setup(service => service.Add(It.IsAny<Product>())).ReturnsAsync(true);

            var result = await _productController.Add(product) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsType<OkObjectResult>(result);

            var json = JsonConvert.SerializeObject(result.Value);
            var value = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.True((bool)value.success);
            Assert.Equal($"Produto {product.ProductName} com preço {product.ProductPrice} adicionado com sucesso!", (string)value.data);
        }

        [Fact(DisplayName = "Nao Deve Adicionar Produto Com Preco Negativo")]
        public async Task NaoDeveAdicionarProdutoComPrecoNegativo()
        {
            var product = new Product { ProductName = "produto teste", ProductPrice = -10m };
            _productServiceMock.Setup(service => service.Add(It.IsAny<Product>())).ThrowsAsync(new ArgumentException("O valor do produto não pode ser negativo."));

            var result = await _productController.Add(product) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.IsType<BadRequestObjectResult>(result);

            var json = JsonConvert.SerializeObject(result.Value);
            var value = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.False((bool)value.success);
            Assert.Contains("O valor do produto não pode ser negativo.", value.errors.ToObject<List<string>>());
        }

        [Fact(DisplayName = "Deve Retornar Todos os Produtos")]
        public async Task DeveRetornarTodosOsProdutos()
        {
            var products = new List<Product>
            {
                new Product { ProductName = "Produto 1", ProductPrice = 10m },
                new Product { ProductName = "Produto 2", ProductPrice = 20m }
            };
            _productServiceMock.Setup(service => service.GetAll()).ReturnsAsync(products);

            var actionResult = await _productController.GetAll();
            var result = actionResult.Result as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsType<OkObjectResult>(result);

            var json = JsonConvert.SerializeObject(result.Value);
            var value = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.True((bool)value.success);

            var productList = value.data.ToObject<List<Product>>();

            Assert.Equal(products.Count, productList.Count);
            for (int i = 0; i < products.Count; i++)
            {
                Assert.Equal(products[i].ProductName, productList[i].ProductName);
                Assert.Equal(products[i].ProductPrice, productList[i].ProductPrice);
                Assert.Equal(products[i].CreatedAt, productList[i].CreatedAt);
                Assert.Equal(products[i].Inventory, productList[i].Inventory);
                Assert.Equal(products[i].IsDeleted, productList[i].IsDeleted);
                Assert.Equal(products[i].ProductId, productList[i].ProductId);
            }
        }

        [Fact(DisplayName = "Deve Retornar Produto Não Encontrado Quando ProductId Não Encontrado")]
        public async Task DeveRetornarProdutoNaoEncontrado()
        {
            var productId = Guid.NewGuid();
            _productServiceMock.Setup(service => service.GetById(productId)).ThrowsAsync(new KeyNotFoundException("Produto não encontrado."));

            var actionResult = await _productController.GetById(productId);
            var result = actionResult.Result as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.IsType<NotFoundObjectResult>(result);

            var json = JsonConvert.SerializeObject(result.Value);
            var value = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.False((bool)value.success);
            Assert.Contains("Produto não encontrado.", value.errors.ToObject<List<string>>());
        }

        [Fact(DisplayName = "Deve Fazer Atualizacao Se Preco Positivo E Valido")]
        public async Task DeveFazerAtualizacaoSePrecoPositivoEValido()
        {
            var productId = Guid.NewGuid();
            var product = new Product { ProductId = productId, ProductName = "produto teste", ProductPrice = 10m };
            _productServiceMock.Setup(service => service.Update(It.IsAny<Product>())).ReturnsAsync(product);

            var result = await _productController.Update(productId, product) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsType<OkObjectResult>(result);

            var json = JsonConvert.SerializeObject(result.Value);
            var value = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.True((bool)value.success);
            Assert.Equal("Produto atualizado com sucesso!", (string)value.data);
        }

        [Fact(DisplayName = "Nao Deve Fazer Atualizacao Se Preco Negativo")]
        public async Task NaoDeveFazerAtualizacaoSePrecoNegativo()
        {
            var productId = Guid.NewGuid();
            var product = new Product { ProductId = productId, ProductName = "produto teste", ProductPrice = -10m };
            _productServiceMock.Setup(service => service.Update(It.IsAny<Product>())).ThrowsAsync(new ArgumentException("O valor do produto não pode ser negativo."));

            var result = await _productController.Update(productId, product) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.IsType<BadRequestObjectResult>(result);

            var json = JsonConvert.SerializeObject(result.Value);
            var value = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.False((bool)value.success);
            Assert.Contains("O valor do produto não pode ser negativo.", value.errors.ToObject<List<string>>());
        }

        [Fact(DisplayName = "Deve Retornar Verdadeiro Quando Produto Deletado Com Sucesso")]
        public async Task DeveRetornarVerdadeiroQuandoProdutoDeletadoComSucesso()
        {
            var productId = Guid.NewGuid();
            _productServiceMock.Setup(service => service.Delete(productId)).ReturnsAsync(true);

            var result = await _productController.Delete(productId) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsType<OkObjectResult>(result);

            var json = JsonConvert.SerializeObject(result.Value);
            var value = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.True((bool)value.success);
            Assert.Equal("Produto deletado com sucesso!", (string)value.data);
        }

        [Fact(DisplayName = "Deve Retornar Produto Não Encontrado Quando Tentativa Deletar Produto Não Encontrado")]
        public async Task DeveRetornarProdutoNaoEncontradoQuandoTentativaDeletarProdutoNaoEncontrado()
        {
            var productId = Guid.NewGuid();
            _productServiceMock.Setup(service => service.Delete(productId)).ThrowsAsync(new KeyNotFoundException("Produto não encontrado."));

            var result = await _productController.Delete(productId) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.IsType<NotFoundObjectResult>(result);

            var json = JsonConvert.SerializeObject(result.Value);
            var value = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.False((bool)value.success);
            Assert.Contains("Produto não encontrado.", value.errors.ToObject<List<string>>());
        }
    }
}
