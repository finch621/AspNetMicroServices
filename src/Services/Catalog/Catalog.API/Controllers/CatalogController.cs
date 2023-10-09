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
    private readonly ILogger _logger;

    public CatalogController(IProductRepository productRepository, ILogger logger)
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

    [HttpGet("{id:length(24)}", Name = "GetProduct")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> GetProduct(string id)
    {
        var product = await _repository.GetProductById(id);
        if (product == null)
        {
            _logger.LogError($"No product found for id:{id}");
            return NotFound();
        }

        return Ok(product);
    }

    [HttpGet("{name}", Name = "GetProductByName")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> GetProductByName(string name)
    {
        var product = await _repository.GetProductByName(name);
        if (product == null)
        {
            _logger.LogError($"No product found named:{name}");
            return NotFound();
        }

        return Ok(product);
    }

    [HttpGet("category/{name}", Name = "GetProductByCategory")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> GetProductByCategory(string name)
    {
        var product = await _repository.GetProductByCategory(name);
        if (product == null)
        {
            _logger.LogError($"No product found with category:{name}");
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task CreateProduct(Product product)
    {
        await _repository.CreateProduct(product);
    }

    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> UpdateProduct(Product product)
    {
        var result = await _repository.UpdateProduct(product);

        if (!result)
        {
            _logger.LogError($"No product found with id:{product.Id}");
            return NotFound();
        }

        return Ok(result);
    }

    [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> DeleteProduct(string id)
    {
        var result = await _repository.DeleteProduct(id);

        if (!result)
        {
            _logger.LogError($"No product found with id:{id}");
            return NotFound();
        }

        return Ok(result);
    }
}
