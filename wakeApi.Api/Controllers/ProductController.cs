using Microsoft.AspNetCore.Mvc;
using wakeDomain.Domain.Interfaces.Service;
using wakeDomain.Domain.Models;

[ApiController]
[Route("api/[controller]")]
public class ProductController : BaseController
{
    private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Add([FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await _productService.Add(product);

            if (result)
                return CustomResponse($"Produto {product.ProductName} com preço {product.ProductPrice} adicionado com sucesso!");

            return CustomResponseError("Erro ao tentar adicionar produto.");

        }

        [HttpPut("update/{productId}")]
        public async Task<IActionResult> Update(Guid productId, [FromBody] Product product)
        {
            if (productId != product.ProductId)
                return CustomResponseError("Produto não encontrado.");

            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var updatedProduct = await _productService.Update(product);

            if (updatedProduct == null)
                return CustomResponseError("Produto não encontrado.");

            return CustomResponse("Produto atualizado com sucesso!");
        }

        [HttpDelete("delete/{productId}")]
        public async Task<IActionResult> Delete(Guid productId)
        {
            var result = await _productService.Delete(productId);
            if (result)
                return CustomResponse("Produto deletado com sucesso!");

            return CustomResponseError("Produto não encontrad.");
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var products = await _productService.GetAll();
            return CustomResponse(products);
        }

        [HttpGet("getById/{productId}")]
        public async Task<ActionResult<Product>> GetById(Guid productId)
        {
            var product = await _productService.GetById(productId);
            if (product == null)
                return CustomResponseError("Produto não encontrado.");

            return CustomResponse(product);
        }

        [HttpGet("orderByType")]
        public async Task<ActionResult<IEnumerable<Product>>> OrderByType([FromQuery] string type)
        {
            var products = await _productService.OrderByType(type);
            return CustomResponse(products); ;
        }

        [HttpGet("searchByName")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchByName([FromQuery] string name)
        {
            var products = await _productService.SearchByName(name);
            return CustomResponse(products); ;
        }
}