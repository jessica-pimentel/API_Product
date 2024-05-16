using Microsoft.AspNetCore.Mvc;
using wakeDomain.Domain.Interfaces.Service;
using wakeDomain.Domain.Models;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var products = await _productService.GetAll();
            return Ok(products);
        }

        [HttpGet("getById")]
        public async Task<ActionResult<Product>> GetById(Guid id)
        {
            var product = await _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("orderByType")]
        public async Task<ActionResult<IEnumerable<Product>>> OrderByType([FromQuery] string field)
        {
            var products = await _productService.OrderByType(field);
            return Ok(products);
        }

        [HttpGet("searchByName")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchByName([FromQuery] string name)
        {
            var products = await _productService.SearchByName(name);
            return Ok(products);
        }
}