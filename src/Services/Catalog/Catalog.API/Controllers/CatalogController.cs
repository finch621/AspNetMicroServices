using System.Net;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;
[ApiController]
[Route("api/v1/catalog")]
public class CatalogController : ControllerBase
{
    private readonly IProductRepository _repository;
    private readonly ILogger<CatalogController> _logger;

    public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
    {
        _repository = productRepository ?? throw new ArgumentNullException();
        _logger = logger ?? throw new ArgumentNullException();
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _repository.GetProducts();
        return Ok(products);
    }

    [HttpGet("{id:length(24)}", Name = "GetProductById")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> GetProductById(string id)
    {
        var product = await _repository.GetProductById(id);
        if (product == null)
        {
            _logger.LogError($"No product found with id:{id}");
            return NotFound();
        }

        return Ok(product);
    }

    [HttpGet("name/{name}", Name = "GetProductByName")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> GetProductByName(string name)
    {
        var product = await _repository.GetProductByName(name);
        if (product == null)
        {
            _logger.LogError($"No product found with name:{name}");
            return NotFound();
        }

        return Ok(product);
    }

    [HttpGet("category/{name}", Name = "GetProductsByCategory")]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string name)
    {
        var products = await _repository.GetProductsByCategory(name);

        return Ok(products);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> CreateProduct([FromBody] Product product)
    {
        await _repository.CreateProduct(product);

        return CreatedAtRoute("GetProductById", new { id = product.Id }, product);
    }

    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateProduct([FromBody] Product product)
    {
        return Ok(await _repository.UpdateProduct(product));
    }

    [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        return Ok(await _repository.DeleteProduct(id));
    }
}
