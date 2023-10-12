using System.Net;
using Basket.API.Entities;
using Basket.API.Repositories;
using Discount.API.GrpcServices;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/basket")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _repository;
    private readonly ILogger<BasketController> _logger;
    private readonly DiscountGrpcService _discountGrpcService;

    public BasketController(IBasketRepository basketRepository, ILogger<BasketController> logger, DiscountGrpcService discountGrpcService)
    {
        _repository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));

    }

    [HttpGet("{userName}", Name = "GetBasket")]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
    {
        var basket = await _repository.GetBasket(userName);

        return Ok(basket ?? new ShoppingCart(userName));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
    {
        foreach (var item in basket.Items)
        {
            if (item.ProductName != null)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }
        }
        return Ok(await _repository.UpdateBasket(basket));
    }

    [HttpDelete("{userName}", Name = "DeleteBasket")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        await _repository.DeleteBasket(userName);
        return Ok();
    }
}
