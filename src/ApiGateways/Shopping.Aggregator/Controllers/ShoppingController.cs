using System.Net;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;

namespace Shopping.Aggregator.Controllers;

[ApiController]
[Route("/api/v1/shopping")]
public class ShoppingController : ControllerBase
{
    private readonly ICatalogService _catalogService;
    private readonly IBasketService _basketService;
    private readonly IOrderService _orderService;

    public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService)
    {
        _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
        _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }

    [HttpGet("{userName}", Name = "GetShoppingByUserName")]
    [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ShoppingModel>> GetShoppingByUserName(string userName)
    {
        var basket = await _basketService.GetBasketByUserName(userName);

        if (basket == null)
            return NotFound();

        foreach (var item in basket.Items)
        {
            var productInfo = await _catalogService.GetCatalogById(item.ProductId);

            if (productInfo != null)
            {
                item.Category = productInfo.Category;
                item.Summary = productInfo.Summary;
                item.Description = productInfo.Description;
                item.ImageFile = productInfo.ImageFile;
            }
        }

        var orders = await _orderService.GetOrdersByUserName(userName);

        return new ShoppingModel()
        {
            UserName = userName,
            BasketWithProducts = basket,
            Orders = orders,
        };
    }
}
